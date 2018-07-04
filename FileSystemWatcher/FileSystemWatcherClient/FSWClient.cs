using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.ServiceModel;
using Newtonsoft.Json;

namespace FSW
{
    public interface IFSWClient { }

    public enum FSWClientExecState
    {
        ConnectWait,   // Pause before connecting
        Connect,       // Signal the connect process to start
        Connecting,    // Attempt to connect
        Connected,     // Connected
        KeepAlive,     // Send a KeepAlive. State reverts back to Connected.
        Disconnect,    // Signal the disconnect process
        Disconnecting, // Wait while disconnecting
        Disconnected   // Connection closed and timer stopped
    }

    public class FSWClientMessageEventArgs : EventArgs
    {
        public FSWClientMessageEventArgs(FSWClientMessage msg)
        {
            Message = msg;
        }

        public FSWClientMessage Message { get; private set; }
    }

    public enum FSWClientMessageEventType
    {
        Debug,
        Info,
        Warning,
        Error,
        Send,
        Receive,
        Metric
    }

    public class FSWClientMessage
    {
        /// <summary>
        /// Add a debug message
        /// </summary>
        /// <param name="message"></param>
        public FSWClientMessage(string message) : this(FSWClientMessageEventType.Debug, message, null) { }

        /// <summary>
        /// Add a general message with no exception data
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="message"></param>
        public FSWClientMessage(FSWClientMessageEventType eventType, string message) : this(eventType, message, null) { }

        /// <summary>
        /// Add a message with included exception data
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public FSWClientMessage(FSWClientMessageEventType eventType, string message, Exception ex)
        {
            EventType = eventType;
            Message = message;
            Exception = ex;
        }

        public FSWClientMessageEventType EventType { get; private set; }
        public string Message { get; private set; }
        public Exception Exception { get; private set; }
    }

    public delegate void FSWCallbackDelegate(Packet packet);

    public class FSWCallbackEvent : IFSWServiceCallback
    {
        public event FSWCallbackDelegate CallbackEvent;

        public void Callback(Packet packet)
        {
//            Body body = packet.Body as Body;
//            packet.Body = body.Deserialise();

            if (CallbackEvent != null)
                CallbackEvent(packet);
        }
    }

    public class FSWClient : IFSWClient, INotifyPropertyChanged, IDisposable
    {
        #region Initialisation

        Object _callbackQueueLock = new Object();

        Timer _stateTimer = null;
        FSWClientExecState _state = FSWClientExecState.Disconnected;
        bool _isRunning = false, _isConnected = false, _monitorCallbackQueue = false;
        int _timerInterval = 10;
        FSWClientMetrics _metrics = null;
        Dictionary<string, PropertyBag> _sessionMetrics = new Dictionary<string,PropertyBag>();
        List<AsyncRequestWrapper> _cbList;
        Guid _sessionKey = Guid.Empty;
        DuplexChannelFactory<IFSWService> _factory;
        FSWCallbackEvent _callback;
        Queue<Packet> _callbackQueue;
        Thread _callbackQueueMonitor;
        PacketHandler _pktHandler;

        enum ReconnectType { RECONNECT, DELAYRECONNECT, NORECONNECT };

        public FSWClient(string provider, int deviceCode) : base() 
        {
            setProvider(provider);
            setDeviceCode(deviceCode);

            _pktHandler    = new PacketHandler(); 
            _callbackQueue = new Queue<Packet>();
            _cbList        = new List<AsyncRequestWrapper>();
            _factory       = new DuplexChannelFactory<IFSWService>(_callback, "FSWServer");
            _stateTimer    = new Timer(new TimerCallback(OnStateTimer));
            
            _callback   = new FSWCallbackEvent();
            _callback.CallbackEvent += _callback_CallbackEvent;

            _callbackQueueMonitor = new Thread(new ThreadStart(monitorCallbackQueue));
            _monitorCallbackQueue = true;
            _callbackQueueMonitor.Start();
        }

        #endregion

        #region Callback Handling

        void _callback_CallbackEvent(Packet packet)
        {
            lock (_callbackQueue)
            {
                _callbackQueue.Enqueue(packet);
            }
        }

        /// <summary>
        /// Thread method to continually monitor the callback queue
        /// </summary>
        void monitorCallbackQueue()
        {
            while (_monitorCallbackQueue)
            {
                if (_callbackQueue.Count > 0)
                {
                    Packet[] packets;

                    // Lock the queue and copy the existing objects to an array.
                    lock (_callbackQueueLock)
                    {
                        int len = _callbackQueue.Count;
                        packets = new Packet[len];
                        _callbackQueue.CopyTo(packets, 0);
                    }

                    // Process the packets outside the lock.
                    foreach (Packet packet in packets)
                    {
                        // A response will have a request waiting in the pending request list which needs to be updated
                        if (packet.Header.PacketType == 1) // Response
                        {
                            foreach (AsyncRequestWrapper req in _cbList)
                            {
                                if (req.SetResponse(packet))
                                    break;
                            }
                        }
                        else
                            Log.Debug(String.Format("Message: {0}", JsonConvert.SerializeObject(packet.Body)));
                    }
                }
            }
        }

        #endregion 

        #region Private Classes

        /// <summary>
        /// Used to store settings used buy the client
        /// </summary>
        static class Settings
        {
            static int _requestTimeout = 3000, _keepAliveInterval = 10;
            static HashCalc.HashType _hashType = HashCalc.HashType.SHA1;
            static string _privateKey = "ABC123";

            public static string Provider { get; set; }
            public static int DeviceCode { get; set; }

            public static int RequestTimeout { get { return _requestTimeout; } set { _requestTimeout = value; } }
            public static int KeepAliveInterval { get { return _keepAliveInterval; } set { _keepAliveInterval = value; } }
 
            public static HashCalc.HashType HashType { get { return _hashType; } set { _hashType = value; } }

            public static string FromSystem { get { return Environment.MachineName; } }
            public static string ToSystem { get { return "fswsrv"; } }

            public static string PrivateKey { get { return _privateKey; } set { _privateKey = value; } }
 
            public static string SessionToken { get; set; }
        }

        /// <summary>
        /// Used to count errors by type received from the FSW server.
        /// </summary>
        public class ErrorCounter : SortedDictionary<int, int>
        {
            public void Increment(int code)
            {
                if (this.ContainsKey(code))
                    this[code]++;
                else
                    this.Add(code, 1);
            }

            public PropertyBag ToPropertyBag()
            {
                PropertyBag pb = new PropertyBag();

                foreach (KeyValuePair<int, int> kvp in this)
                {
                    pb.Add(kvp.Key.ToString(), kvp.Value);
                }

                return pb;
            }
        }

        /// <summary>
        /// Used to store session metrics such as bytes in/out, errors etc.
        /// </summary>
        public class FSWClientMetrics
        {
            ErrorCounter _errorCounter = new ErrorCounter();

            public FSWClientMetrics()
            {
                StartDT = DateTime.Now;
            }

            public DateTime StartDT { get; private set; }
            public DateTime EndDT   { get; set; }

            public int ConnectCount { get; set; }
            public int RequestCount { get; set; }
            public int ResponseCount { get; set; }
            public int RequestErrorCount { get; set; }
            public int ResponseErrorCount { get; set; }
            public int BytesSent { get; set; }
            public int BytesReceived { get; set; }

            public ErrorCounter ErrorCount { get { return _errorCounter; } }

            /// <summary>
            /// Return the metrics as a property bag
            /// </summary>
            /// <returns></returns>
            public PropertyBag GetMetrics()
            {
                PropertyBag pb = new PropertyBag();

                pb.Add("StartDT", StartDT);
                pb.Add("EndDT", EndDT);

                pb.Add("ConnectCount", ConnectCount);
                pb.Add("RequestCount", RequestCount);
                pb.Add("RequestErrorCount", RequestErrorCount);
                pb.Add("ResponseCount", ResponseCount);
                pb.Add("ResponseErrorCount", ResponseErrorCount);
                pb.Add("BytesSent", BytesSent);
                pb.Add("BytesReceived", BytesReceived);

                pb.Add("ErrorCounters", _errorCounter.ToPropertyBag());

                return pb;
            }
        }

        /// <summary>
        /// Used for internal counters.
        /// </summary>
        static class Counter
        {
            static int _commandId = 0;
            
            public static int CommandId { get { return _commandId++; } }
            public static int ConnectWait { get; set; }
            public static int RunStep { get; set; }
            public static int KeepAliveTicks { get; set; }
        }

        #endregion

        #region Public properties

        public Guid SessionKey { get { return _sessionKey; } }

        void setSessionKey(Guid value)
        {
            _sessionKey = value;
            notifyPropertyChanged("SessionKey");
        }

        public string Provider { get { return Settings.Provider; } set { setProvider(value); } }

        void setProvider(string value)
        {
            Settings.Provider = value;
            notifyPropertyChanged("Provider");
        }

        public int DeviceCode { get { return Settings.DeviceCode; } set { setDeviceCode(value); } }

        void setDeviceCode(int value)
        {
            Settings.DeviceCode = value;
            notifyPropertyChanged("DeviceCode");
        }

        /// <summary>
        /// This is set to true when the state timer is running following a connect and false following a disconnect.
        /// </summary>
        public bool IsRunning { get { return _isRunning; } }

        void setIsRunning(bool value)
        {
            if (value != _isRunning)
            {
                _isRunning = value;
                notifyPropertyChanged("IsRunning");
            }
        }

        /// <summary>
        /// Set to true when the client has successfully completed the chanllenge response and false following a disconnect
        /// </summary>
        public bool IsConnected { get { return _isConnected; } set { setIsConnected(value); } }

        void setIsConnected(bool value)
        {
            if (_isConnected != value)
            {
                _isConnected = value;
                notifyPropertyChanged("IsConnected");
            }
        }

        public FSWClientExecState State { get { return _state; } }

        /// <summary>
        /// Used to manage state changes
        /// </summary>
        /// <param name="state"></param>
        void setState(FSWClientExecState state)
        {
            if (state != _state)
            {
                notifyMessageEvent(new FSWClientMessageEventArgs(new FSWClientMessage(
                    String.Format("State changed from {0} to {1}", _state, state))));

                _state = state;

                if (_state == FSWClientExecState.Connected || _state == FSWClientExecState.KeepAlive)
                    Counter.KeepAliveTicks = 0;
                else if (_state == FSWClientExecState.ConnectWait)
                    Counter.ConnectWait = 0;

                if (_state == FSWClientExecState.Connected || _state == FSWClientExecState.KeepAlive)
                {
                    setIsConnected(true);
                }
                
                notifyPropertyChanged("State");
            }
        }

        #endregion

        #region State manager

        /// <summary>
        /// The state machine
        /// </summary>
        /// <param name="state"></param>
        void OnStateTimer(object state)
        {
            switch (_state)
            {
                // These states are just wait states and don't require any action.
                case FSWClientExecState.Disconnected:
                case FSWClientExecState.Disconnecting:
                case FSWClientExecState.Connecting:
                    break;
                case FSWClientExecState.Connect:
                    setState(FSWClientExecState.Connecting);
                    connect();
                    break;
                case FSWClientExecState.Connected:
                    if (getSeconds(++Counter.KeepAliveTicks) >= Settings.KeepAliveInterval)
                        setState(FSWClientExecState.KeepAlive);
                    break;
                case FSWClientExecState.KeepAlive:
                    setState(FSWClientExecState.Connected);
                    SendKeepAlive();
                    break;
                case FSWClientExecState.ConnectWait:
                    // Wait until the counter is exceeded before reconnecting
                    if (getSeconds(++Counter.ConnectWait) >= 5)
                        setState(FSWClientExecState.Connect);
                    break;
                case FSWClientExecState.Disconnect:
                    setState(FSWClientExecState.Disconnecting);
                    disconnect(ReconnectType.NORECONNECT);
                    break;
                default:
                    break;
            }
        }

        int getSeconds(int counter)
        {
            return counter / (1000 / _timerInterval);
        }

        #endregion

        #region Packet Handling

        /// <summary>
        /// Initiate the connection process and start the state timer
        /// </summary>
        public void Connect()
        {
            if (IsConnected)
                disconnect(ReconnectType.RECONNECT);
            else
                setState(FSWClientExecState.Connect);

            if (!_isRunning)
            {
                _stateTimer.Change(_timerInterval, _timerInterval);
                setIsRunning(true);
            }
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        private void connect()
        {
            setState(FSWClientExecState.Connecting);

            if (_metrics == null)
                _metrics = new FSWClientMetrics();

            ServiceProxy<IFSWService> proxy = null;

            try
            {
                proxy = new ServiceProxy<IFSWService>(_factory.CreateChannel());

                IFSWService svc = proxy.Channel;

                setSessionKey(svc.Connect(Settings.Provider, Settings.DeviceCode));
                setState(FSWClientExecState.Connected);
            }
            catch (Exception ex)
            {
                handleError(ex);
            }
            finally
            {
                if (proxy != null) proxy.Dispose();
            }

            if (State != FSWClientExecState.Connected)
                setState(FSWClientExecState.ConnectWait);
        }
            
        void handleError(Exception ex)
        {
            Type t = ex.GetType();

            notifyMessageEvent(new FSWClientMessageEventArgs(new FSWClientMessage(
                    FSWClientMessageEventType.Error,
                    String.Format("Error: {0}->{1}", ex.GetType(), ex.Message), ex)));

            _metrics.RequestErrorCount++;

            if (ex is WebException)
            {
                disconnect(ReconnectType.DELAYRECONNECT);
            }
        }

        void handleError(Packet rsp)
        {
            if (rsp == null)
            {
                notifyMessageEvent(new FSWClientMessageEventArgs(new FSWClientMessage(
                    FSWClientMessageEventType.Error,
                    "A null value was returned from a request")));
            }
            else
            {
                if (rsp.Body is FaultData)
                {
                    FaultData fd = rsp.Body as FaultData;

                    notifyMessageEvent(new FSWClientMessageEventArgs(new FSWClientMessage(
                        FSWClientMessageEventType.Error,
                        String.Format("Code={0}, Msg={1}", fd.Code, fd.Message))));

                    _metrics.ErrorCount.Increment(fd.Code);
                }
                else
                {
                }
            }
            
            _metrics.ResponseErrorCount++;
        }

        public void SendKeepAlive()
        {
            ServiceProxy<IFSWService> proxy = null;

            try
            {
                proxy = new ServiceProxy<IFSWService>(_factory.CreateChannel());

                if (proxy.Channel.KeepAlive(_sessionKey))
                    Counter.KeepAliveTicks = 0;
                else
                    setState(FSWClientExecState.ConnectWait);
            }
            catch (Exception ex)
            {
                handleError(ex);
            }
            finally
            {
                proxy.Dispose();
            }
        }

        /// <summary>
        /// Initiate the disconnection process
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected)
                setState(FSWClientExecState.Disconnect);
        }

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        void disconnect()
        {
            disconnect(ReconnectType.NORECONNECT);
        }
        
        /// <summary>
        /// Disconnect from the server
        /// </summary>
        void disconnect(ReconnectType reconnectType)
        {
            if (IsConnected)
            {
                ServiceProxy<IFSWService> proxy = null;

                try
                {
                    proxy = new ServiceProxy<IFSWService>(_factory.CreateChannel());

                    proxy.Channel.Disconnect(_sessionKey);

                    // Stop the timer if the restart was triggered by a user action
                    if (reconnectType == ReconnectType.NORECONNECT)
                    {
                        _stateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        setIsRunning(false);
                    }
                }
                catch (Exception ex)
                {
                    handleError(ex);
                }
                finally
                {
                    proxy.Dispose();
                }
            }

            if (reconnectType == ReconnectType.DELAYRECONNECT)
                setState(FSWClientExecState.ConnectWait);
            else if (reconnectType == ReconnectType.RECONNECT)
                setState(FSWClientExecState.Connect);
            else
                setState(FSWClientExecState.Disconnected);
        }

        T deserialiseResultString<T>(object result)
        {
            return JsonConvert.DeserializeObject<T>(result.ToString());
        }

        #endregion

        #region Packet Transmission

        /// <summary>
        /// Send a synchronous request
        /// </summary>
        /// <param name="req">A request packet</param>
        /// <returns>A response packet</returns>
        public Packet SendRequest(Packet req)
        {
            Packet rsp = null;

            if (IsConnected)
            {
                ServiceProxy<IFSWService> proxy = null;

                try
                {
                    proxy = new ServiceProxy<IFSWService>(_factory.CreateChannel());
                    rsp = proxy.Channel.Execute(req);
                    Counter.KeepAliveTicks = 0;
                }
                catch (Exception ex)
                {
                    handleError(ex);
                }
                finally
                {
                    proxy.Dispose();
                }
            }

            return rsp;
        }

        public void SendRequestAsync(Packet req, Action<Packet> callback)
        {
            if (IsConnected)
            {
                ServiceProxy<IFSWService> proxy = null;

                try
                {
                    proxy = new ServiceProxy<IFSWService>(_factory.CreateChannel());
                    proxy.Channel.ExecuteAsync(req);
                    Counter.KeepAliveTicks = 0;
                }
                catch (Exception ex)
                {
                    handleError(ex);
                }
                finally
                {
                    proxy.Dispose();
                }
            }
        }

        #endregion

        #region Event Handling

        public event EventHandler<FSWClientMessageEventArgs> MessageEvent;

        void notifyMessageEvent(FSWClientMessageEventArgs e)
        {
            if (MessageEvent != null) MessageEvent(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
