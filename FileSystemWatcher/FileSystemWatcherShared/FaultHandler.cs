using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using ProtoBuf;

namespace FSW
{
    public enum ErrorCode
    {
        /// <summary>
        /// When adding new codes you must also add a description to the ErrorMessages constructor
        /// </summary>
        GeneralError             = 1000,
        CommunicationError       = 1001,
        DatabaseConnectionError  = 1002,
        GeneralDatabaseError     = 1003,
        ArgumentError            = 1004,
        AuthenticationError      = 1005,
        AuthorisationError       = 1006,
        FaultException           = 2000,
    }

    class ErrorMessages : Dictionary<ErrorCode, string>
    {
        public ErrorMessages()
        {
            this.Add(ErrorCode.GeneralError,            "A general error has occurred");
            this.Add(ErrorCode.CommunicationError,      "A communication error has occurred");
            this.Add(ErrorCode.DatabaseConnectionError, "An error occurred while attempting to establish a database connection: Handler={0}");
            this.Add(ErrorCode.GeneralDatabaseError,    "An error occurred during database command execution");
            this.Add(ErrorCode.ArgumentError,           "An argument was either missing or invalid");
            this.Add(ErrorCode.AuthenticationError,     "Authentication error: {0}");
            this.Add(ErrorCode.AuthorisationError,      "Authorisation error: {0}");
            this.Add(ErrorCode.FaultException,          "Unhandled error during {0} call");
        }
    }

    [DataContract]
    public class ErrorMessage
    {
        public ErrorMessage(Exception ex)
        {
            if (ex != null)
            {
                bool hasInnerEx = true;

                string msg = ex.Message;
                StackTrace = ex.StackTrace;

                while (hasInnerEx)
                {
                    ex = ex.InnerException;
                    if (ex == null)
                        hasInnerEx = false;
                    else
                        msg = String.Concat(msg, "\n", "-> Inner Exception: ", ex.Message);
                }

                Message = msg;
            }
        }

        [DataMember]
        public string Message    { get; private set; }
        
        [DataMember]
        public string StackTrace { get; private set; }

        public override string ToString()
        {
            string msg = "";

            if (Message != null)
            {
                msg = Message;

                if (StackTrace != null)
                {
                    msg += StackTrace;
                }
            }

            return msg;
        }
    }

    [DataContract]
    public class FaultData
    {
        [DataMember] 
        public string Message      { get; set; }
        
        [DataMember]
        public int Code { get; set; }
        
        [DataMember]
        public ErrorMessage ErrorMessage { get; set; }

        public override string ToString()
        {
            string str = String.Format("{0}; Code={1}{2}",
                Message, Code, ErrorMessage != null ? "; ErrorMsg=" + ErrorMessage.ToString() : "");

            return str;
        }
    }

    public static class FaultHandler
    {
        static ErrorMessages _messages = new ErrorMessages();

        static bool _excludeExceptionData = false;

        /// <summary>
        /// Include exception data in response packets
        /// </summary>
        public static bool ExcludeExceptionData { get { return _excludeExceptionData; } set { _excludeExceptionData = value; } }

        public static FaultData GetFaultData(string message)
        {
            return new FaultData() { Message = message, Code = (int)ErrorCode.GeneralError };
        }

        public static FaultData GetFaultData(string message, Exception ex)
        {
            return new FaultData() { Message = message, Code = (int)ErrorCode.GeneralError, ErrorMessage = new ErrorMessage(ex) };
        }

        public static FaultData GetFaultData(ErrorCode code)
        {
            return GetFaultData(code, null, null);
        }

        public static FaultData GetFaultData(ErrorCode code, params object[] messageArgs)
        {
            return GetFaultData(code, null, messageArgs);
        }

        public static FaultData GetFaultData(ErrorCode code, Exception ex, params object[] messageArgs)
        {
            string msg = String.Empty;

            try
            {
                msg = _messages[code];

                if (messageArgs != null)
                    msg = String.Format(msg, messageArgs);
            }
            catch
            {
                msg = "An undefined error has occurred";
                
                if (messageArgs != null && messageArgs.Length > 0)
                {
                    msg += ": ";

                    for (int x = 0; x < messageArgs.Length; x++)
                        msg = String.Concat(msg, " Arg", x, "=", messageArgs[x], ";");
                }
            }

            return new FaultData() { Message = msg, Code = (int)code, ErrorMessage = new ErrorMessage(ex) };
        }

        public static string GetMessage(ErrorCode code)
        {
            return _messages[code];
        }
    }
}
