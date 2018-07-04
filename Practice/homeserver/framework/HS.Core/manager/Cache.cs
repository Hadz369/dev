using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HS
{
    public static class Cache
    {
        static CodeCache _codes = new CodeCache();

        static SessionManager _sessions = new SessionManager();

        public static CodeCache Codes { get { return _codes; } }

        public static SessionManager Sessions { get { return _sessions; } }

        public static bool IsAuthorised(string method, int token)
        {
            return true;
        }
    }
}
