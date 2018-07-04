using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Metro.Data
{
    internal class MemberHandler : IDisposable
    {
        DbHandler _db = null;

        public MemberHandler(DbHandler db)
        {
            _db = db;
        }

        public Response ProcessRequest(Request request)
        {
            switch (request.Header.Action)
            {
                case "GetMemberList":
                    return GetMemberList(request);
                case "GetMemberTiers":
                    return GetMemberTierList(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid action: ", request.Header.Action)));
            }
        }

        private Response GetMemberList(Request request)
        {
            Response response = new Response(request.Header);

            using (SqlCommand cmd = _db.GetCommand(CommandType.StoredProcedure, "dbo.tpMemberExport"))
            {
                try
                {
                    DateTime dateFrom = request.PropertyBag.GetProperty("DateFrom").GetValue<DateTime>();
                    cmd.Parameters.AddWithValue("DateFrom", dateFrom);
                }
                catch { /* datetime parameter not supplied*/ }

                response.Data = XmlSerialiser.DataTableToXml(_db.Execute(cmd, "MemberList"));
            }

            return response;
        }

        private Response GetMemberTierList(Request request)
        {
            Response response = new Response(request.Header);

            using (SqlCommand cmd = _db.GetCommand(CommandType.StoredProcedure, "dbo.tpMemberInterface"))
            {
                cmd.Parameters.AddWithValue("Action", 3);

                response.Data = XmlSerialiser.DataTableToXml(_db.Execute(cmd, "MemberAtLocation"));
            }

            return response;
        }

        public void Dispose()
        {
        }
    }
}

