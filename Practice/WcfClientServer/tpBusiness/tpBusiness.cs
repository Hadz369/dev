using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.ThirdParty
{
    /// <summary>
    /// The business handler is a singleton used to coordinate sessions as well as
    /// providing a managed layer between the service and database
    /// </summary>
    public class TP_BusinessHandler
    {
        #region Singleton Initialisation

        static readonly object _mylock = new object();
        static TP_BusinessHandler _instance = null;

        private TP_BusinessHandler() { }

        public static TP_BusinessHandler Instance
        {
            get
            {
                lock (_mylock)
                {
                    if (_instance == null) _instance = new TP_BusinessHandler();

                    return _instance;
                }
            }
        }

        #endregion

        TP_DataHandler _data;
        int _session = 0;

        /// <summary>
        /// Set up the data connection
        /// </summary>
        /// <param name="constr"></param>
        public void Initialise(string constr)
        {
            _data = new TP_DataHandler(constr);
        }

        /// <summary>
        /// Get a new session
        /// </summary>
        /// <returns></returns>
        public SessionInfo GetSession()
        {
            return new SessionInfo(++_session);
        }

        /// <summary>
        ///  Get the site name
        /// </summary>
        /// <returns></returns>
        public string GetSiteName()
        {
            return _data.GetSiteName();
        }
    }

    class SessionDetail
    {
        public int SessionId;
        public DateTime Started;
        public DateTime LastUsed;
    }

    class SessionHandler
    {
        List<int> _sessions = new List<int>();

        int _sessionId = 0;
        /*
        public int Start()
        {            
        }

        public int Start(int session)
        {
        }

        int GetSession(int session)
        {
            if (_sessions.Contains(session))
                return session;

        }
        */
    }
}
