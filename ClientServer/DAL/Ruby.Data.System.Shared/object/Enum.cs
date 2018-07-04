using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;

namespace Ruby.Data
{
    public enum ServiceState
    {
        Stopped           = 0,
        Starting          = 1,
        DbConnecting      = 2,
        SvcConnecting     = 3,
        Started           = 4,
        Stopping          = 5
    }
}
