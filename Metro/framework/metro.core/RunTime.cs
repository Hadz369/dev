using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Runtime.InteropServices;

namespace Metro
{
    public static class RunTime
    {
        public static Guid GetAssemblyGuid()
        {
            var attribute = (GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0];

            return Guid.Parse(attribute.Value);
        }
    }
}
