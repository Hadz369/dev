using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Metro.Data
{
    internal class MachineHandler : IDisposable
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
                case "GetMachineDetails":
                    return GetMachineDetails(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid data handler action: ", request.Header.Action)));
            }
        }

        private Response GetMachineDetails(Request request)
        {
            Response response = new Response(request.Header);

            string sql = "" +
                    "select " +
                    "  mdet.M_ID as DeviceID," +
                    "  mdet.GMID," +
                    "  mdet.Serial as SerialNo," +
                    "  mdet.Name as GameName," +
                    "  mdet.Club_ID as HouseNo," +
                    "  mdet.LocnNumber as BaseNo," +
                    "  mdet.Denomination_Code as Denomination," +
                    "  'Club' as Location," +
                    "  manf.LDesc as Manufacturer," +
                    "  case when mdet.MTGM_ID > 0 then 1 else 0 end as IsMTGM " +
                    "from EPS_Game.dbo.MDETL mdet " +
                    "inner join dbo.Manf_Codes manf on mdet.Manf = manf.Manf " +
                    "inner join dbo.Denominations dnom on mdet.Denomination_Code = dnom.Denom_Code " +
                    "where mdet.status = 1";

            SqlCommand cmd = _db.GetCommand(CommandType.Text, sql);

            response.Data = _db.Execute(cmd, "MachineDetails");

            return response;
        }

        public void Dispose()
        {
        }
    }
}
