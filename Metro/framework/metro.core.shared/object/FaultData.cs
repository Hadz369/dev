using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Metro
{
    public enum FaultCode
    {
        Unauthorised = 40960,
        UnmanagedError = 40961,
        ParameterError = 40962,
        DatabaseError = 40963,
        CommunicationError = 40964,
        IOError = 40965,
    }

    [DataContract]
    public class FaultData
    {
        public FaultData() { }

        public FaultData(FaultCode code, string message)
        {
            Init(code, message, null);
        }

        public FaultData(FaultCode code, string message, Exception exception)
        {
            Init(code, message, exception);
        }

        private void Init(FaultCode code, string message, Exception exception)
        {
            Code = code;
            Message = message;
            Exception = exception;
        }

        [DataMember]
        public FaultCode Code { get; private set; }

        [DataMember]
        public string Message { get; private set; }

        [DataMember]
        public Exception Exception { get; set; }
    }


}
