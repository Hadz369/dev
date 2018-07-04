using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ProtoBuf;

namespace FSW
{
    public class SubscriberCollection : List<Session>
    {
        Object _lock = new Object();

        public SubscriberCollection(string channelName)
        {
            ChannelName = channelName.Trim() == String.Empty ? "Undefined" : channelName;
        }

        public string ChannelName { get; private set; }

        public bool Subscribe(Guid sessionKey)
        {
            try
            {
                var session = Cache.Sessions[sessionKey];

                if (session.IsOpen)
                {
                    lock (_lock)
                    {
                        this.Add(session);
                    }
                    return true;
                }
                else
                {
                    Log.Debug("Cannot subscribe when the session is closed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(String.Format("Error subscribing: Type = {0}, Ex = {1}", ex.GetType(), ex.Message));
                return false;
            }
        }

        public bool Unsubscribe(Guid sessionKey)
        {
            try
            {
                var session = Cache.Sessions[sessionKey];
                lock (_lock)
                {
                    this.Remove(session);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(String.Format("Error unsubscribing: Type = {0}, Ex = {1}", ex.GetType(), ex.Message));
                return false;
            }
        }

        public int SendPacket(Packet packet)
        {
            List<Session> removeList = null;
            int count = 0;

            foreach (var session in this)
            {
                try
                {
                    session.SendCallback(packet);
                    count++;
                }
                catch (CommunicationException ex)
                {
                    Log.Debug(String.Format("Error sending callback to a subscriber of the {0} message queue. Msg = {1}", ChannelName.ToLower(), ex.Message));

                    removeList.Add(session);
                }
            }

            if (removeList != null)
            {
                foreach (Session session in removeList)
                {
                    lock (_lock)
                    {
                        this.Remove(session);
                        session.Callback = null;
                    }
                }
            }

            return count;
        }
    }

    public class SubscriberChannelCollection : ConcurrentDictionary<string, SubscriberCollection>
    {
        public void Send(Packet packet, string[] channels)
        {
            foreach (string ch in channels)
            {
                try
                {
                    var collection = this[ch];
                    collection.SendPacket(packet);
                }
                catch { /* collection not found */ }
            }
        }

        public void Subscribe(Guid sessionKey, string[] channels)
        {
            foreach (string c in channels)
            {
                try
                {
                    var collection = this[c];
                    collection.Subscribe(sessionKey);
                }
                catch (KeyNotFoundException) 
                {
                    Log.Debug(String.Format("Error subscribing to channel {0} for sessionKey = {1}. Msg=Channel key not found", c, sessionKey));
                }
            }
        }

        public void Unsubscribe(Guid sessionKey, string[] channels)
        {
            foreach (string c in channels)
            {
                try
                {
                    var collection = this[c];
                    collection.Unsubscribe(sessionKey);
                }
                catch (KeyNotFoundException)
                {
                    Log.Debug(String.Format("Error unsubscribing from channel {0} for sessionKey = {1}. Msg=Channel key not found", c, sessionKey));
                }
            }
        }
    }
}
