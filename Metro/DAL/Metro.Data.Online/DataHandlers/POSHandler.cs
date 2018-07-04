using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Metro.Data
{
    internal class POSHandler : IDisposable
    {
        DbHandler _db = null;

        public POSHandler(DbHandler db)
        {
            _db = db;
        }

        public Response ProcessRequest(Request request)
        {
            switch (request.Header.Action)
            {
                case "GetMemberInfo":
                    return GetMemberInfo(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid data handler action: ", request.Header.Action)));
            }
        }

        private Response GetMemberInfo(Request request)
        {
            Response response = new Response(request.Header);

            string cardNumber = request.PropertyBag.GetProperty("CardNumber").GetValue<string>();

            SqlCommand cmd = _db.GetCommand(CommandType.StoredProcedure, "dbo.usp_POS_Sel_MemberInfo");
            cmd.Parameters.AddWithValue("CardNumber", cardNumber);

            //response.Data = XmlSerialiser.DataTableToXml( _db.Execute(cmd, "MemberInfo"));
            response.Data = _db.Execute(cmd, "MemberInfo");

            return response;
        }

        public void Dispose()
        {
        }
    }
}
