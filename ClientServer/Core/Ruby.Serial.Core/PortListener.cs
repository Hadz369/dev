using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;
using Ruby.Core;

namespace Ruby.Serial
{
    enum ListenerState
    {
        Stopped,
        Starting,
        StartWait,
        Listening,
        Stopping,
    }

    public delegate void PacketReceived_Handler(string packet);
    public delegate void MessageEvent_Handler(TracerData data);

    public class PortListener
    {
        bool _isRunning = false, _stopListener = false;
        string _logPath, _datPath;
        int _timerInterval = 100;
        ListenerState _state = ListenerState.Stopped;
        SerialPort _port = null;
        System.Threading.Timer _wdTimer;
        Thread _listener, _handler;
        SerialPortSettings _settings = null;
        IPortBuffer _buf;

        public event PacketReceived_Handler PacketReceived;
        public event MessageEvent_Handler MessageEvent;

        public PortListener(SerialPortSettings settings, IPortBuffer portBuffer)
        {
            _buf = portBuffer;

            _wdTimer = new System.Threading.Timer(OnTimerFired);

            _datPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "cms\\xml");
            _logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "cms\\log");
        }

        #region Public Properties

        public bool IsRunning { get { return _isRunning; } }

        #endregion

        #region Event Handlers

        private void OnTimerFired(object stateInfo)
        {
            StateHandler();
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            if (_state == ListenerState.Stopped)
            {
                _state = ListenerState.Starting;

                _wdTimer.Change(0, _timerInterval);
            }
            else
            {
                SendMessage(new TracerData(System.Diagnostics.TraceLevel.Warning, "Start method called listener while not in the stopped state"));
            }
        }

        public void Stop()
        {
            SendMessage(new TracerData("Stopping the port listener"));
            _stopListener = true;

            SendMessage(new TracerData("Waiting for worker threads to complete running tasks"));
            _listener.Join();
            _handler.Join();

            _state = ListenerState.Stopped;
        }

        public void Restart()
        {
            Stop();

            _state = ListenerState.Starting;
        }

        #endregion

        #region Private Methods

        private void StateHandler()
        {
            int timerDelay = _timerInterval;

            try
            {
                _wdTimer.Change(Timeout.Infinite, Timeout.Infinite);

                switch (_state)
                {
                    case ListenerState.StartWait:
                        _state = ListenerState.Starting;
                        // Wait for 30 seconds before trying again
                        timerDelay = 30000;
                        break;
                    case ListenerState.Starting:
                        StartListener();
                        break;
                    case ListenerState.Listening:
                        break;
                    case ListenerState.Stopping:
                        Stop();
                        break;
                    default:   /* ListenerState.Stopped */
                        break;
                }
            }
            catch (Exception ex)
            {
                SendMessage(new TracerData("Error in state machine", ex));
            }

            _wdTimer.Change(timerDelay, _timerInterval);
        }

        private void StartListener()
        {
            SendMessage(new TracerData("Starting serial port listener"));
            
            // Set the state to listening here to avoid repeated calls from the main loop
            // while the listener is still starting
            _state = ListenerState.Listening;
            
            _listener = new Thread(new ThreadStart(Listener));
            _listener.Start();

            _handler = new Thread(new ThreadStart(ProcessBuffer));
            _handler.Start();
        }

        private void Listener()
        {
            try
            {
                if (_port != null)
                {
                    _port.Close();
                    _port.Dispose();
                }

                try
                {
                    _port = new SerialPort(
                        _settings.PortName, _settings.BaudRate, _settings.Parity,
                        _settings.DataBits, _settings.StopBits);

                    _port.ReadBufferSize = _settings.ReadBufferSize;
                    _port.Encoding = _settings.Encoding;

                    _port.Open();
                }
                catch (Exception ex)
                {
                    _state = ListenerState.StartWait;
                    SendMessage(new TracerData("Error opening serial port", ex));
                }

                if (_state == ListenerState.Listening)
                {
                    while (!_stopListener)
                    {
                        try
                        {
                            if (_port.BytesToRead > 0)
                            {
                                int len = _port.BytesToRead;

                                // Only read 1KB at a time. Why? I just like doing it this way to avoid possible
                                // long waits when the stop method is called.
                                if (len > 1024) len = 1024;

                                byte[] bytes = new byte[len];

                                int x = _port.Read(bytes, 0, len);

                                _buf.Add(bytes);
                            }
                        }
                        catch (Exception ex)
                        {
                            SendMessage(new TracerData("Error reading serial port data", ex));
                        }

                        Thread.Sleep(100);
                    }

                    try
                    {
                        _port.Close();
                    }
                    catch (Exception ex)
                    {
                        SendMessage(new TracerData("Error closing serial port", ex));
                    }
                }
            }
            catch(Exception ex)
            {
                SendMessage(new TracerData("Error during port listener execution", ex));
            }
        }

        private void SendMessage(TracerData data)
        {
            if (MessageEvent != null)
            {
                MessageEvent(data);
            }
        }

        private void SendPacket(string packet)
        {
            if (PacketReceived != null)
            {
                PacketReceived(packet);
            }
            else
            {
                SendMessage(new TracerData(
                    System.Diagnostics.TraceLevel.Warning,
                    String.Concat("No packet subscribers found. XML packet discarded; Length=", packet.Length)));
            }
        }

        private void ProcessBuffer()
        {
            while (!_stopListener)
            {
                try
                {
                    if (_buf.HasData && _buf.HasChanges)
                    {
                        string xml = _buf.GetPacket();

                        if (xml != String.Empty)
                        {
                            SendMessage(new TracerData(String.Concat("XML Data Loaded; Length=", xml.Length)));

                            SendPacket(xml);
                        }
                    }
                }
                catch(Exception ex)
                {
                    SendMessage(new TracerData("Error during buffer processing.", ex));
                }

                Thread.Sleep(1);
            }
        }

        #endregion
    }
}
