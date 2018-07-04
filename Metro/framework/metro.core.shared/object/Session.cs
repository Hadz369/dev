using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Metro
{
    [DataContract(Namespace = "")]
    public class Session
    {
        public Session() { }

        public Session(string vendor, int device, string sessionkey)
        {
            Vendor = vendor;
            DeviceId = device;
            SessionKey = sessionkey;
            Created = LastUsed = DateTime.Now;
        }

        [DataMember]
        public string Vendor { get; private set; }
        [DataMember]
        public int DeviceId { get; private set; }
        [DataMember]
        public string SessionKey { get; private set; }
        [DataMember]
        public DateTime Created { get; private set; }
        [DataMember]
        public DateTime LastUsed { get; set; }
    }

    [CollectionDataContract(Namespace = "", Name = "Sessions", ItemName = "Session")]
    public class SessionCollection : List<Session>
    {
        public SessionCollection() : base() { }

        public SessionCollection(Session[] items)
            : base()
        {
            foreach (Session item in items)
            {
                Add(item);
            }
        }

        public Session Find(string sessionkey)
        {
            Session session = null;

            foreach (Session s in this)
            {
                if (s.SessionKey == sessionkey)
                {
                    session = s;
                    break;
                }
            }

            return session;
        }

        public Session Find(string vendor, int device)
        {
            Session session = null;

            foreach (Session s in this)
            {
                if (s.Vendor == vendor && s.DeviceId == device)
                {
                    session = s;
                    break;
                }
            }

            return session;
        }
    }
}
