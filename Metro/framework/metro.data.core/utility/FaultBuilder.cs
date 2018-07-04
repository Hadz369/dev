using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Metro;
using System.ServiceModel;

namespace Metro.Data
{
    static class ExType
    {
        public const string DatabaseError = "DatabaseError";
        public const string IOError       = "FileAccessError";
        public const string GeneralError  = "GeneralError";
    }

    public static class FaultBuilder
    {
        public static FaultException<FaultData> GetGeneralFault(int code, string message, string methoddata, Exception ex)
        {
            FaultData fault = new FaultData(code, GetExType(ex), message);

            // Log the error to the trace handlers
            Tracer.Error(String.Format("{0}; MethodData={1}", fault.ToString(), methoddata), ex);

            return new FaultException<FaultData>(fault);
        }

        private static string GetExType(Exception ex)
        {
            Type t = ex.GetType();

            if (t == typeof(System.Data.SqlClient.SqlException))
                return ExType.DatabaseError;
            else if (t == typeof(System.IO.IOException))
                return ExType.IOError;
            else
                return ExType.GeneralError;
        }
    }
}
