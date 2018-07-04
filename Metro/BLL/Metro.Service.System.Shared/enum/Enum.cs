using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;

namespace Metro.Service
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

    public enum RequestErrorCode
    {
        Unauthorised       = 0xA000,
        UnmanagedError     = 0xA001,
        ParameterError     = 0xA002,
        DatabaseError      = 0xA003,
        CommunicationError = 0xA004,
        IOError            = 0xA005,
    }
}
