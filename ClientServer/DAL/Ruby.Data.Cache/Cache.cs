using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ruby.Data
{
    public static class Cache
    {
        static CodeCache _codes = new CodeCache();

        public static CodeCache Codes { get { return _codes; } }
    }
}
