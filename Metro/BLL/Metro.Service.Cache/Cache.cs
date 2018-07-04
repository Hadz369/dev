using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Metro.Service
{
    public static class Cache
    {
        static CodeCache _codes = new CodeCache();
        static SessionManager _sessionMgr = new SessionManager();

        public static CodeCache Codes { get { return _codes; } }

        public static SessionManager SessionManager { get { return _sessionMgr; } }

        public static bool IsAuthorised(string method, int token)
        {
            return true;
        }
    }
}
