using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS
{
    public static class SvcLogger
    {
        static BusinessServiceType _type;

        public static BusinessServiceType Type { get { return _type; } set { _type = value; } }
 
        public static string Get(string handler, string action, object message)
        {
            return String.Format("{0} {1,16} {2, 45} {3}", _type, handler, action, message);
        }
    }
}
