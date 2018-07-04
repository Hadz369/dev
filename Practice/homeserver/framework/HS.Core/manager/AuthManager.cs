using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS
{
    public class AuthManager : IDisposable
    {
        /*
        Dictionary<string, List<string>> _authdict = new Dictionary<string, List<string>>();

        SessionCollection _sessions = new SessionCollection();

        Object lockobj = new Object();

        public AuthManager()
        {
            _vendordevices.Add("TestVendor1", new List<int>());
            _vendordevices["TestVendor1"].Add(101);
            _vendordevices["TestVendor1"].Add(102);
            _vendordevices["TestVendor1"].Add(103);

            _vendordevices.Add("TestVendor2", new List<int>());
            _vendordevices["TestVendor2"].Add(201);
            _vendordevices["TestVendor2"].Add(202);

            _timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTimer));
        }

        public SessionCollection Sessions
        {
            get { return _sessions; }
        }

        private void StartTimer()
        {
            _timer.Change(_sessionTimeout, _sessionTimeout);
        }

        private void StopTimer()
        {
            _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }

        public void OnTimer(object stateinfo)
        {
            ExpireSessions();
        }

        public Session CreateSession(string vendor, int deviceid)
        {
            Session s = null;

            lock (lockobj)
            {
                if (_vendordevices.ContainsKey(vendor))
                {
                    if (_vendordevices[vendor].Contains(deviceid))
                    {
                        // Return an existing session if it exists
                        s = _sessions.Find(vendor, deviceid);
                        if (s != null)
                        {
                            //Trace.WriteLine(String.Concat("Session Reused:  Vendor=", s.Vendor, ", Device=", s.DeviceId.ToString(), ", Key=", s.SessionKey));
                            s.LastUsed = DateTime.Now;
                        }
                        else
                        {
                            s = new Session(vendor, deviceid, GetSessionId());
                            //FnTrace.WriteLine(String.Concat("Session Created: Vendor=", s.Vendor, ", Device=", s.DeviceId.ToString(), ", Key=", s.SessionKey));
                            _sessions.Add(s);
                        }
                    }
                }

                // Start the timer when the first item is inserted
                if (_sessions.Count == 1) StartTimer();
            }

            return s;
        }

        /// <summary>
        /// Create a 4 byte session id represented as an 8 character hex string
        /// </summary>
        /// <returns></returns>
        private string GetSessionId()
        {
            byte[] bytes;
            string key = "";

            // Generate a random number big enough to use 4 bytes
            Random rnd = new Random();
            bytes = BitConverter.GetBytes(rnd.Next(2000000000, 2100000000));

            // Convert each byte to a 2 character hex value and append to the output string
            foreach (byte b in bytes)
                key += b.ToString("X2");

            // Make sure the session is not already used
            Session s = _sessions.Find(key);

            // If it is used (which should be ridiculously rare) calculate keys until it is unique
            while (s != null)
                key = GetSessionId();

            return key;
        }

        public bool ValidateSession(string sessionkey)
        {
            bool ok = false;

            lock (lockobj)
            {
                Session session = _sessions.Find(sessionkey);

                if (session != null)
                {
                    session.LastUsed = DateTime.Now;
                    ok = true;
                }
            }

            return ok;
        }

        public void CloseSession(string sessionkey)
        {
            lock (lockobj)
            {
                Session session = _sessions.Find(sessionkey);

                if (session != null)
                {
                    _sessions.Remove(session);
                }
            }
        }

        private void ExpireSessions()
        {
            ExpireSessions(false);
        }

        private void ExpireSessions(bool serviceClosing)
        {
            lock (lockobj)
            {
                Tracer.Info("Managing idle connections");
                DateTime expdt = DateTime.Now.AddMilliseconds(-(_sessionTimeout));

                List<Session> list = null;

                foreach (Session s in _sessions)
                {
                    if (serviceClosing || s.LastUsed < expdt)
                    {
                        if (list == null) list = new List<Session>();
                        list.Add(s);
                    }
                }

                if (list != null)
                {
                    foreach (Session s in list)
                    {
                        Tracer.Info(String.Concat("Session Closed:  Vendor=", s.Vendor, ", Device=", s.DeviceId.ToString(), ", Key=", s.SessionKey));
                        _sessions.Remove(s);
                    }
                }

                // Stop the timer when no items exist
                if (_sessions.Count == 0) StopTimer();
            }
        }

        public void Dispose()
        {
            StopTimer();
            ExpireSessions(true);

            _timer.Dispose();
            _vendordevices = null;
            _sessions = null;
        }
    */
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
