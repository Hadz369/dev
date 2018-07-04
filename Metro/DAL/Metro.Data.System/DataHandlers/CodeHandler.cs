using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using Metro;

namespace Metro.Data
{
    internal class CodeHandler : IDisposable
    {
        DbHandler _db = null;

        public CodeHandler(DbHandler db)
        {
            _db = db;
        }

        #region Public Service Methods

        public Response ProcessRequest(Request request)
        {
            switch (request.Header.Action)
            {
                case "GetCodesByCodeType":
                    return GetCodesByCodeType(request);
                case "GetCodesByCodeGuid":
                    return GetCodesByCodeGuid(request);
                case "GetCodeChanges":
                    return GetCodeChanges(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid Action: ", request.Header.Action)));
            }
        }

        private Response GetCodesByCodeType(Request request)
        {
            Response response = new Response(request.Header);

            const string P_TYPEDEFN = "TypeDefn";

            Dictionary<string, Property> parms = new Dictionary<string, Property>();
            parms.Add(P_TYPEDEFN, null);

            try
            {
                request.ValidateProperties(parms);

                if (parms[P_TYPEDEFN] != null)
                {
                    response.Data = XmlSerialiser.DataTableToXml(GetCodes(parms[P_TYPEDEFN].GetValue<String>()));
                }
                else
                {
                    throw new InvalidPropertyException("Missing one or more required parameters");
                }
            }
            catch (InvalidPropertyException ex)
            {
                response.Data = new FaultData(FaultCode.ParameterError, "Parameter Error", ex);
            }
            catch (SqlException ex)
            {
                response.Data = new FaultData(FaultCode.DatabaseError, "Database Error", ex);
            }
            catch (Exception ex)
            {
                response.Data = new FaultData(FaultCode.UnmanagedError, "Unmanaged Error", ex);
            }

            return response;
        }

        private Response GetCodesByCodeGuid(Request request)
        {
            Response response = new Response(request.Header);

            const string CODEGUID = "CodeGuid";

            Dictionary<string, Property> parms = new Dictionary<string, Property>();
            parms.Add(CODEGUID, null);

            try
            {
                request.ValidateProperties(parms);

                if (parms[CODEGUID] != null)
                {
                    response.Data = XmlSerialiser.DataTableToXml(GetCodes(parms[CODEGUID].GetValue<Guid>()));
                }
                else
                {
                    throw new InvalidPropertyException("Missing one or more required parameters");
                }
            }
            catch (InvalidPropertyException ex)
            {
                response.Data = new FaultData(FaultCode.ParameterError, "Parameter Error", ex);
            }
            catch (SqlException ex)
            {
                response.Data = new FaultData(FaultCode.DatabaseError, "Database Error", ex);
            }
            catch (Exception ex)
            {
                response.Data = new FaultData(FaultCode.UnmanagedError, "Unmanaged Error", ex);
            }

            return response;
        }

        private Response GetCodeChanges(Request request)
        {
            Response response = new Response(request.Header);

            const string LASTREFRESH = "LastRefresh";

            Dictionary<string, Property> parms = new Dictionary<string, Property>();
            parms.Add(LASTREFRESH, null);

            try
            {
                request.ValidateProperties(parms);

                if (parms[LASTREFRESH] != null)
                {
                    response.Data = XmlSerialiser.DataTableToXml(GetCodes(parms[LASTREFRESH].GetValue<DateTime>()));
                }
                else
                {
                    throw new InvalidPropertyException("Missing one or more required parameters");
                }
            }
            catch (InvalidPropertyException ex)
            {
                response.Data = new FaultData(FaultCode.ParameterError, "Parameter Error", ex);
            }
            catch (SqlException ex)
            {
                response.Data = new FaultData(FaultCode.DatabaseError, "Database Error", ex);
            }
            catch (Exception ex)
            {
                response.Data = new FaultData(FaultCode.UnmanagedError, "Unmanaged Error", ex);
            }

            return response;
        }

        #endregion

        #region Private Database Methods

        private DataTable GetCodeType(string typeDefn)
        {
            string sql = "" +
                "select CodeTypeId, CodeTypeDefn, CodeTypeName, CodeTypeDesc " +
                "from met.CodeType " +
                "where CodeTypeDefn = @CodeTypeDefn";

            SqlCommand cmd = _db.GetCommand(CommandType.Text, sql);

            cmd.Parameters.AddWithValue("CodeTypeDefn", typeDefn);

            return _db.Execute(cmd);
        }

        private DataTable GetCodes(DateTime lastRefresh)
        {
            string sql = "" +
                "select " +
                "  t.CodeTypeId, t.TypeDefn, c.CodeGuid, c.CodeId, c.CodeDefn, c.CodeName," +
                "  c.CodeDesc, c.CodeValue, c.Sequence " +
                "from met.CodeType t " +
                "inner join met.Code c on t.CodeTypeId = c.CodeTypeId " +
                "where c.updated >= @lastRefresh";

            SqlCommand cmd = _db.GetCommand(CommandType.Text, sql);

            cmd.Parameters.AddWithValue("lastRefresh", lastRefresh);

            return _db.Execute(cmd);
        }

        private DataTable GetCodes(string typeDefn)
        {
            string sql = "" +
                "select" +
                "  t.CodeTypeId, t.TypeDefn, c.CodeGuid, c.CodeId, c.CodeDefn, c.CodeName," +
                "  c.CodeDesc, c.CodeValue, c.Sequence " +
                "from met.CodeType t " +
                "inner join met.Code c on t.CodeTypeId = c.CodeTypeId " +
                "where t.TypeDefn = @CodeType";

            SqlCommand cmd = _db.GetCommand(CommandType.Text, sql);

            cmd.Parameters.AddWithValue("CodeType", typeDefn);

            return _db.Execute(cmd);
        }

        private DataTable GetCodes(Guid codeGuid)
        {
            string sql = "" +
                "select" +
                "  t.CodeTypeId, t.TypeDefn, c.CodeGuid, c.CodeId, c.CodeDefn, c.CodeName," +
                "  c.CodeDesc, c.CodeValue, c.Sequence " +
                "from met.CodeType t " +
                "inner join met.Code c1 on t.CodeTypeId = c1.CodeTypeId " +
                "inner join met.Code c2 on c1.CodeTypeId = c2.CodeTypeId " +
                "where c2.CodeGuid = @CodeGuid";

            SqlCommand cmd = _db.GetCommand(CommandType.Text, sql);

            cmd.Parameters.AddWithValue("CodeGuid", codeGuid);

            return _db.Execute(cmd);
        }

        #endregion

        public void Dispose()
        {
        }
    }
}
