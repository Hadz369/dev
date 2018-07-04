using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Ruby.Data
{
    public class CMSSqlBuilder
    {
        public SqlCommand ReadsInsert(DbHandler db, int snapId, DateTime readDt, int devicesRead, int devicesChanged, int linksRead, int linksChanged)
        {
            string sql = "" +
                "insert into dbo.Reads (SnapId, ReadDate, DevicesRead, DevicesChanges, LinksRead, LinksChanged) " +
                "values (@SnapId, @ReadDate, @DevicesRead, @DevicesChanged, @LinksRead, @LinksChanged)";

            SqlCommand cmd = db.GetCommand(CommandType.Text, sql);

            cmd.Parameters.AddWithValue("SnapId", snapId); 
            cmd.Parameters.AddWithValue("ReadDate", readDt);
            cmd.Parameters.AddWithValue("DeviceRead", devicesRead);
            cmd.Parameters.AddWithValue("DeviceChanged", devicesChanged);
            cmd.Parameters.AddWithValue("LinksRead", linksRead);
            cmd.Parameters.AddWithValue("LinksChanged", linksChanged);

            return cmd;
        }

        public SqlCommand CmsMeterInsert(DbHandler db, int snapId, CMSMeterReadBase read)
        {
            string sql1 = "", sql2 = "";
            string table = (read.MeterSetType == CMSMeterSetType.Egm ? "dbo.EGM" : "dbo.Links");


            sql1 = String.Format("insert into {0} (SnapId, GMID, ReadDate", table);
            sql2 = "values (@SnapId, @ReadDate, @DevicesRead";

            SqlParameter[] parms = new SqlParameter[read.Meters.Count];

            int x = 0;

            foreach (Meter m in read.Meters)
            {
                sql1 += String.Concat(", ",  m.Name);
                sql2 += String.Concat(", @", m.Name);
             
                parms[x++] = new SqlParameter(m.Name, m.Value);
            }

            sql1 = String.Format("{0}) {1})", sql1, sql2);

            SqlCommand cmd = db.GetCommand(CommandType.Text, sql1);
            cmd.Parameters.AddRange(parms);

            return cmd;
        }
    }
}
