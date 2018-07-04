using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Metro.Data
{
    internal class SnapshotHandler
    {
        DbHandler _db = null;

        public SnapshotHandler(DbHandler db)
        {
            _db = db;
        }

        public Response ProcessRequest(Request request)
        {
            switch (request.Header.Action)
            {
                case "InsertSnap":
                    return InsertSnap(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid Action: ", request.Header.Action)));
            }
        }


        private Response InsertSnap(Request request)
        {
            Response response = new Response(request.Header);
            DateTime snapDT = DateTime.Now;

            int snapType = request.PropertyBag.GetProperty("SnapType").GetValue<int>();

            Property p = request.PropertyBag.GetProperty("SnapDT");
            if (p != null)
                snapDT = p.GetValue<DateTime>();

            SqlCommand cmd = _db.GetCommand(CommandType.StoredProcedure, "dev.Snap_Insert");
            cmd.Parameters.AddWithValue("SnapType", snapType);
            cmd.Parameters.AddWithValue("SnapDT", snapDT);

            SqlParameter rc = new SqlParameter("RC", SqlDbType.Int);
            rc.Direction = ParameterDirection.ReturnValue;

            response.Data = rc.Value;

            return response;
        }
    }
}
