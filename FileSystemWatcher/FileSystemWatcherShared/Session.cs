using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using ProtoBuf;
using System.ServiceModel;

namespace FSW
{
    [ProtoContract]
    public class Session
    {
        IFSWServiceCallback _callback = null;
        Object _lock = new Object();

        /// <summary>
        /// This constructor is required by the serialiser and is not intended for general use.
        /// </summary>
        public Session() { }

        public Session(Guid sessionKey, string providerKey, int deviceCode)
        {
            SessionKey = sessionKey;
            ProviderKey = providerKey;
            DeviceCode = deviceCode;
            Opened = DateTime.UtcNow;
            IsOpen = true;

            ModuleList = new List<string>();
            ModuleList.Add("Communication");
            ModuleList.Add("TestCommand1");
            ModuleList.Add("TestCommand2");

            FunctionList = new List<string>();
            FunctionList.Add("Subscribe");
            FunctionList.Add("Unsubscribe");
            FunctionList.Add("GetStuffForCommand1");
            FunctionList.Add("GetStuffForCommand2");
        }

        // Required
        [ProtoMember(1)]
        public Guid SessionKey { get; private set; }
        [ProtoMember(2)]
        public string ProviderKey { get; private set; }
        [ProtoMember(3)]
        public int DeviceCode { get; private set; }

        // States
        [ProtoMember(10)]
        public bool IsOpen { get; set; }
        [ProtoMember(11)]
        public bool IsExpired { get; set; }

        // General
        [ProtoMember(30)]
        public DateTime Opened { get; set; }
        [ProtoMember(31)]
        public DateTime Closed { get; set; }
        [ProtoMember(32)]
        public DateTime LastActivity { get; set; }

        [ProtoMember(70)]
        public int PacketsIn { get; set; }
        [ProtoMember(71)]
        public int PacketsOut { get; set; }
        [ProtoMember(72)]
        public int Callbacks { get; set; }

        [ProtoMember(73)]
        public int BytesIn { get; set; }
        [ProtoMember(74)]
        public int BytesOut { get; set; }

        [ProtoMember(80)]
        public int Reconnections { get; set; }
        [ProtoMember(81)]
        public int Resubscriptions { get; set; }

        [ProtoMember(100)]
        public List<string> ModuleList { get; private set; }
        [ProtoMember(101)]
        public List<string> FunctionList { get; private set; }

        // Callback
        public IFSWServiceCallback Callback { get { return _callback; } set { SetCallback(value); } }

        void SetCallback(IFSWServiceCallback callback)
        {
            bool setNull = true;

            if (callback != null && ((ICommunicationObject)callback).State == CommunicationState.Opened)
            {
                _callback = callback;
                setNull = false;
            }

            if (setNull)
                _callback = null;
        }

        public Guid Reconnect()
        {
            lock (_lock)
            {
                if (IsOpen)
                {
                    Callback = null;
                    SetLastActivity();
                    Reconnections++;
                }
                else throw new CommunicationException("Cannot reconnect to a closed session");
            }

            return SessionKey;
        }

        public void Resubscribe(IFSWServiceCallback callback)
        {
            lock (_lock)
            {
                if (IsOpen)
                {
                    Callback = callback;
                    SetLastActivity();

                    if (callback != null) Resubscriptions++;
                }
                else throw new CommunicationException("Cannot resubscribe to a closed session");
            }
        }

        public void Disconnect()
        {
            lock (_lock)
            {
                SetLastActivity();
                Closed = DateTime.UtcNow;
                Callback = null;
                IsOpen = false;
            }
        }

        public void Expire()
        {
            lock (_lock)
            {
                Closed = DateTime.UtcNow;
                Callback = null;
                IsOpen = false;
                IsExpired = true;
            }
        }

        public bool IsAuthorised(string commandClass, string command)
        {
            if (ModuleList.Contains(commandClass))
                if (FunctionList.Contains(command))
                    return true;

            return false;
        }

        public void SetLastActivity()
        {
            LastActivity = DateTime.UtcNow;
        }

        public void SendCallback(Packet packet)
        {
            if (Callback != null && ((ICommunicationObject)Callback).State == CommunicationState.Opened)
            {
                try
                {
                    Callback.Callback(packet);
                    Callbacks++;
                    SetLastActivity();
                }
                catch (Exception ex)
                {
                    Callback = null;
                    throw new CommunicationException(String.Concat("Error sending callback: Session = ", SessionKey, ", Error = ", ex.Message));
                }
            }
            else throw new CommunicationException(String.Concat("Callback not defined: Session = ", SessionKey));
        }
    }

    public class SessionCollection : ConcurrentDictionary<Guid, Session>
    {
        public Guid Connect(string providerKey, int deviceCode, out Session session)
        {
            if (providerKey != "testProvider" || deviceCode != 121)
            {
                FaultData fd = FaultHandler.GetFaultData(ErrorCode.AuthorisationError, "Access denied");
                throw new FaultException<FaultData>(fd, new FaultReason("Provider key or device code not found"));
            }

            Guid guid = Guid.NewGuid();
            bool connected = false;

            session = null;

            foreach (var kvp in this)
            {
                if (String.Compare(kvp.Value.ProviderKey, providerKey, true) == 0
                    && kvp.Value.DeviceCode == deviceCode)
                {
                    guid = kvp.Value.Reconnect();
                    session = kvp.Value;
                    connected = true;
                }
            }

            if (!connected)
            {
                session = new Session(guid, providerKey, deviceCode);

                while (!this.TryAdd(guid, session)) { }
            }

            return guid;
        }

        enum AuthorisationState
        {
            Authorised = 0,
            NotAuthenticated = 1,
            NotAuthorised = 2,
        }

        public void CheckAuthorisation(Guid sessionKey)
        {
            CheckAuthorisation(sessionKey, String.Empty, String.Empty);
        }

        public void CheckAuthorisation(Packet packet)
        {
            CheckAuthorisation(packet.Header.SessionKey, packet.Header.CommandClass, packet.Header.Command);
        }

        public void CheckAuthorisation(Guid sessionKey, string commandClass, string command)
        {
            try
            {
                var s = this[sessionKey];

                if (s.IsOpen)
                {
                    s.SetLastActivity();

                    if (!s.IsAuthorised(commandClass, command))
                    {
                        FaultData fd = FaultHandler.GetFaultData(ErrorCode.AuthorisationError, "Not authorised");
                        throw new FaultException<FaultData>(fd);
                    }
                }
                else
                {
                    FaultData fd = FaultHandler.GetFaultData(ErrorCode.AuthenticationError, "Access denied");
                    throw new FaultException<FaultData>(fd, new FaultReason("Session not found"));
                }
            }
            catch
            {
                FaultData fd = FaultHandler.GetFaultData(ErrorCode.AuthenticationError, "Access denied");
                throw new FaultException<FaultData>(fd, new FaultReason("No access to the specified command class or command"));
            }
        }
    }

}
