using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Configuration;
using System.IO;
using FlexAdmin.Event;

namespace RestServer
{
    public class SessionManager : IDisposable
    {
        System.Threading.Timer _timer = null;
        Dictionary<string, List<int>> _vendordevices = new Dictionary<string, List<int>>();
        int _sessionTimeout = 300000; // 5 minute default

        SessionCollection _sessions = new SessionCollection();

        Object lockobj = new Object();

        public SessionManager()
        {
            try
            {
                string timeout = ConfigurationManager.AppSettings["SessionTimeout"];
                int x = Convert.ToInt32(timeout);
                if (x > 60000) // 1 minute minimum
                    _sessionTimeout = x;
            }
            catch 
            {
                FnTrace.WriteLine(System.Diagnostics.TraceLevel.Warning, "Invalid session timeout specified in the configuration file. Defaulting to " + _sessionTimeout.ToString());
            }

            _vendordevices.Add("v1", new List<int>());
            _vendordevices["v1"].Add(101);
            _vendordevices["v1"].Add(102);
            _vendordevices["v1"].Add(103);

            _vendordevices.Add("v2", new List<int>());
            _vendordevices["v2"].Add(201);
            _vendordevices["v2"].Add(202);

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
                            FnTrace.WriteLine(String.Concat("Session Reused:  Vendor=", s.Vendor, ", Device=", s.DeviceId.ToString(), ", Key=", s.SessionKey));
                            s.LastUsed = DateTime.Now;
                        }
                        else
                        {
                            s = new Session(vendor, deviceid, GetSessionId());
                            FnTrace.WriteLine(String.Concat("Session Created: Vendor=", s.Vendor, ", Device=", s.DeviceId.ToString(), ", Key=", s.SessionKey));
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
            Session session = _sessions.Find(sessionkey);

            if (session != null)
            {
                _sessions.Remove(session);
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
                FnTrace.WriteLine("Managing idle connections");
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
                        FnTrace.WriteLine(String.Concat("Session Closed:  Vendor=", s.Vendor, ", Device=", s.DeviceId.ToString(), ", Key=", s.SessionKey));
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
    }

    public sealed class Biz : IDisposable
    {
        Data _db;
        SessionManager _sessionmgr = new SessionManager();

        #region Singleton Initialisation

        private static Object initlock = new Object();

        private static volatile Biz _instance = null;

        private Biz(string constring)
        {
            _db = new Data(constring);
        }

        public static void Init(string constring)
        {
            if (_instance == null)
            {
                lock (initlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Biz(constring);
                    }
                }
            }
        }

        private static Biz Initialise()
        {
            if (_instance == null)
            {
                lock (initlock)
                {
                    if (_instance == null)
                    {
                        try
                        {
                            string constr = "";
                            ConfigureTrace();
                            FnTrace.WriteLine("Initialisation");
                            constr = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                            _instance = new Biz(constr);
                        }
                        catch(Exception ex)
                        {
                            string msg = "Error during service initialisation";
                            FnTrace.WriteLine(System.Diagnostics.TraceLevel.Error, msg);
                            throw (new Exception(msg, ex));
                        }
                    }
                }
            }

            return _instance;
        }

        static void ConfigureTrace()
        {
            string logPath; 
            
            try
            {
                System.Diagnostics.TraceLevel level = System.Diagnostics.TraceLevel.Info;

                if (ConfigurationManager.AppSettings.AllKeys.Contains("TraceLevel_File"))
                {
                    int l = Convert.ToInt32(ConfigurationManager.AppSettings["TraceLevel_File"]);
                    level = (System.Diagnostics.TraceLevel)l;
                }

                // Only one instance is allowed so log to the common program files folder.
                logPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "flexinet\\log");

                if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

                // Create a daily file trace listener.
                FnTextWriterTraceListener _logFileDailyListener = new FnTextWriterTraceListener(new FnTraceFileOptions(
                    logPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)) { TraceLevel = level };

                FnTrace.Add(_logFileDailyListener);
            }
            catch (Exception ex)
            {
                FnMethodHelper mh = new FnMethodHelper();
                mh.SetMsg("An error occurred while preparing the trace options", ex);
                FnTrace.WriteLine(mh);
            }
        }

        public static Biz Instance { get { return Initialise(); } }

        #endregion

        public string SignOn(string vendor, string device)
        {
            string key = "";
            int devid;

            using (FnMethodHelper mh = FnTrace.MethodStart("SignOn") )
            {
                if (Int32.TryParse(device, out devid))
                {
                    Session s = _sessionmgr.CreateSession(vendor, devid);
                    if (s != null)
                        key = s.SessionKey;
                }
            }

            return key;
        }

        public void SignOff(string sessionkey)
        {
            _sessionmgr.CloseSession(sessionkey);
        }

        public bool ValidateSession(string sessionkey)
        {
            return _sessionmgr.ValidateSession(sessionkey);
        }

        public SessionCollection GetSessions()
        {
            return _sessionmgr.Sessions;
        }

        public Member GetMember(string badge)
        {
            return _db.GetMember(badge);
        }

        public Member GetMemberAtLocation(string baseNo)
        {
            return _db.GetMemberAtLocation(baseNo);
        }

        public MemberList GetMembers(string surname)
        {
            return _db.GetMembers(surname);
        }

        public Tier GetTier(int tierId)
        {
            return _db.GetTier(tierId);
        }

        public TierList GetTiers()
        {
            return _db.GetTiers();
        }

        public MachineList GetMachines()
        {
            return _db.GetMachines();
        }

        public MachineStats GetCurrentMachineStats(int machineId)
        {
            return _db.GetCurrentMachineStats(machineId);
        }

        public ServiceResponse UpdatePoints(int memberId, AccountUpdateType type, int amount)
        {
            int centsPerPoint, pointValue;
            ServiceResponse _resp = new ServiceResponse();
            
            Int32.TryParse(_db.GetOption(Constants.OPTIONTYPE_PROMOTION, Constants.OPT_PROMOTION_CENTSPERPOINT), out centsPerPoint);
            Int32.TryParse(_db.GetOption(Constants.OPTIONTYPE_PROMOTION, Constants.OPT_PROMOTION_POINTVALUE), out pointValue);

            if (centsPerPoint > 0 && pointValue > 0) // both parameters exist
            {
                try
                {
                    switch (type)
                    {
                        case AccountUpdateType.Issue:
                        case AccountUpdateType.Redeem:
                            amount = amount * 1000;
                            break;
                        case AccountUpdateType.Earn:
                            break;
                        default:
                            break;
                    }

                    _db.UpdatePoints(memberId, type, amount);

                    _resp.ErrorMessage = String.Format("Success: MemberId={0}, Type={1}, AdjustmentAmt={2}", memberId, type, Convert.ToDecimal(amount)/1000);
                }
                catch(SqlException sex)
                {
                    _resp.ReturnCode = -2;
                    _resp.Exception = sex;
                    _resp.ErrorMessage = sex.Message + (sex.InnerException != null ? "; " + sex.InnerException.Message : "");
                }
                catch(Exception ex)
                {
                    _resp.ReturnCode = -1;
                    _resp.Exception = ex;
                    _resp.ErrorMessage = ex.Message + (ex.InnerException != null ? "; " + ex.InnerException.Message : "");
                }
            }
            else
            {
                _resp.ReturnCode = -1;
                _resp.ErrorMessage = "Required promotional options not found";
            }

            return _resp;
        }

        public void Close()
        {
            _sessionmgr.Dispose();
            this.Dispose();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
