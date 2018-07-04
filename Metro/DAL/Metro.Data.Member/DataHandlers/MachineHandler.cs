using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Metro.Data
{
    internal class MachineHandler
    {
        DbHandler _db = null;

        public MachineHandler(DbHandler db)
        {
            _db = db;
        }

        public Response ProcessRequest(Request request)
        {
            switch (request.Header.Action)
            {
                case "GetMachineList":
                    return GetMachineList(request);
                case "GetMemberAtLocation":
                    return GetMemberAtLocation(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid machine handler action: ", request.Header.Action)));
            }
        }

        private Response GetMachineList(Request request)
        {
            Response response = new Response(request.Header);

            SqlCommand cmd = _db.GetCommand(CommandType.StoredProcedure, "dbo.tpGetMachineList");

            response.Data = XmlSerialiser.DataTableToXml( _db.Execute(cmd, "MachineList"));

            return response;
        }

        private Response GetMemberAtLocation(Request request)
        {
            Response response = new Response(request.Header);

            SqlCommand cmd = _db.GetCommand(CommandType.StoredProcedure, "dbo.tpGetMemberAtLocation");

            response.Data = XmlSerialiser.DataTableToXml(_db.Execute(cmd, "MemberAtLocation"));

            return response;
        }

    }
}
