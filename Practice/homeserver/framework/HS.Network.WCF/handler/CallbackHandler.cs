using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;

namespace HS.Network.WCF
{
    enum TimerState
    {
        Started  = 0,
        Stopping = 1,
        Stopped  = 2
    }

    public class CallbackHandler
    {
        Timer _timer;

        #region Singleton Initialisation

        private static CallbackHandler _instance = null;
        private static Object _initlock = new Object(), _clientLock = new Object();
        private Queue<string> _messageQueue = new Queue<string>();
        bool _hasFaultedClients = false, _queueProcessing = false;
        TimerState _timerState = TimerState.Stopped;
        //List<CallbackListItem> _clients = new List<CallbackListItem>();
        Dictionary<Guid, CallbackListItem> _clients = new Dictionary<Guid,CallbackListItem>();

        private CallbackHandler()
        {
        }

        private static CallbackHandler GetInstance()
        {
            if (_instance == null)
            {
                lock (_initlock)
                {
                    if (_instance == null)
                    {
                        _instance = new CallbackHandler();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Returns the singleton instance of the DbConnectionBroker class
        /// </summary>
        public static CallbackHandler Instance { get { return GetInstance(); } }

        #endregion

        public void Start()
        {
            if (_timer == null)
                _timer = new Timer(OnTimer);

            _timer.Change(100, 100);

            _timerState = TimerState.Started;
        }

        public void Stop()
        {
            _timerState = TimerState.Stopping;

            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            _timerState = TimerState.Stopped;
        }

        public void Enque(string msg)
        {
            if (_timerState == TimerState.Started && _clients.Count > 0)
                _messageQueue.Enqueue(msg);
        }

        private void OnTimer(object stateInfo)
        {
            if (!_queueProcessing)
                ProcessQueue();
        }

        private void ProcessQueue()
        {
            _queueProcessing = true;

            while (_messageQueue.Count > 0)
            {
                Send(_messageQueue.Dequeue());
                Thread.Sleep(1);
            }

            _queueProcessing = false;
        }

        private void Send(string msg)
        {
            lock (_clientLock)
            {
                foreach(CallbackListItem i in _clients.Values)
                {
                    if (_timerState != TimerState.Started)
                        break;

                    if (!i.IsFaulted)
                    {
                        try
                        {
                            i.Callback.Callback(msg);
                        }
                        catch(Exception ex)
                        {
                            Tracer.Error("Error sending message", ex);
                            i.IsFaulted = true;
                            _hasFaultedClients = true;
                        }
                    }
                }
            }

            if (_hasFaultedClients)
                RemoveFaultedClients();
        }

        public void Register(Guid guid)
        {
            if (guid != null)
            {
                try
                {
                    lock (_clientLock)
                    {
                        Tracer.Info("Registering client: Guid=" + guid.ToString());

                        if (_clients.ContainsKey(guid))
                        {
                            Tracer.Warning(String.Format("Replacing existing client registration: Guid={0}", guid));
                            _clients.Remove(guid);
                        }

                        IHSCallback callback = OperationContext.Current.GetCallbackChannel<IHSCallback>();
                        _clients.Add(guid, new CallbackListItem(guid, callback));
                        Tracer.Debug("Callback channel registered");
                    }
                }
                catch (Exception ex)
                {
                    Tracer.Error("Error registering client callback", ex);
                    throw ex;
                }
            }
        }

        public void Deregister(Guid guid)
        {
            lock (_clientLock)
            {
                if (_clients.ContainsKey(guid))
                {
                    Tracer.Info("Deregistering client: Guid=" + guid.ToString());
                    _clients.Remove(guid);
                }
            }         
        }

        private void RemoveFaultedClients()
        {
            lock (_clientLock)
            {
                List<Guid> _faulted = new List<Guid>();

                foreach (CallbackListItem i in _clients.Values)
                {
                    if (i.IsFaulted)
                        _faulted.Add(i.Guid);
                }

                Tracer.Debug(String.Concat("Removing ", _faulted.Count, " faulted clients"));

                foreach (Guid g in _faulted)
                {
                    Tracer.Debug(String.Concat("Faulted client: Guid=",  g.ToString()));
                    _clients.Remove(g);
                }

                _hasFaultedClients = false;
            }
        }
    }

    class CallbackListItem
    {
        public CallbackListItem(Guid guid, IHSCallback callback)
        {
            Guid = guid;
            Callback = callback;
        }

        public Guid Guid { get; private set; }
        public IHSCallback Callback { get; private set; }
        public bool IsFaulted { get; set; }
    }
}
