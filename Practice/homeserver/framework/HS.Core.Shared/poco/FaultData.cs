using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace HS
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

            // Format the exception message into a base exception to avoid known type issues
            if (exception != null)
            {
                string exMsg = "";

                Exception ex = exception;
                while (ex.InnerException != null)
                {
                    exMsg += String.Concat("/nInnerMessage=", ex.InnerException.Message);
                    ex = ex.InnerException;
                }

                exMsg = String.Format("Exception: Type={0}, Msg={1}{2}", exception.GetType(), exception.Message, exMsg == String.Empty ? "" : exMsg);

                Exception = new Exception(exMsg);
            }
        }

        [DataMember]
        public FaultCode Code { get; private set; }

        [DataMember]
        public string Message { get; private set; }

        [DataMember]
        public Exception Exception { get; set; }
    }


}
