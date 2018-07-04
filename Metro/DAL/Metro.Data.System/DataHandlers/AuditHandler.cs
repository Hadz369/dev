using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Metro.Data
{
    internal class AuditHandler
    {
        DbHandler _db = null;

        public AuditHandler(DbHandler db)
        {
            _db = db;
        }

        public Response ProcessRequest(Request request)
        {
            switch (request.Header.Action)
            {
                case "InsertAudit":
                    return InsertAudit(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid Action: ", request.Header.Action)));
            }
        }

        private Response InsertAudit(Request request)
        {
            Response response = new Response(request.Header);

            int auditType  = request.PropertyBag.GetProperty("AuditType").GetValue<int>();
            string message = request.PropertyBag.GetProperty("Message").GetValue<string>();

            SqlCommand cmd = _db.GetCommand(CommandType.Text, "" +
                "insert into met.Audit (AuditType, AuditCode, HasExMsg, Message, UID, MID, Created) " +
                "values (@type, 0, 0, @message, @uid, @mid, getdate()); " +
                "SELECT SCOPE_IDENTITY()");


            cmd.Parameters.AddWithValue("type", auditType);
            cmd.Parameters.AddWithValue("message", message);
            cmd.Parameters.AddWithValue("uid", _db.UID);
            cmd.Parameters.AddWithValue("mid", _db.MID);

            response.Data = _db.ExecuteScalar(cmd);

            return response;
        }
    }
}
