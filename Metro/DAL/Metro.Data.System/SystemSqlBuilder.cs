using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Metro.Data
{
    public class SystemSqlBuilder
    {
        #region Device Table

        public SqlCommand DeviceInsert(DbHandler db, int deviceType, int deviceState, int gmid)
        {
            SqlCommand cmd = db.GetCommand(CommandType.StoredProcedure, "dev.Device_Insert");

            cmd.Parameters.AddWithValue("DeviceType", deviceType);
            cmd.Parameters.AddWithValue("DeviceState", deviceState);
            cmd.Parameters.AddWithValue("Gmid", gmid);

            return cmd;
        }

        public SqlCommand DeviceUpdate(DbHandler db, int deviceId, int deviceType, int deviceState, int gmid)
        {
            SqlCommand cmd = db.GetCommand(CommandType.StoredProcedure, "dev.Device_Update");

            cmd.Parameters.AddWithValue("DeviceId", deviceId);
            cmd.Parameters.AddWithValue("DeviceType", deviceType);
            cmd.Parameters.AddWithValue("DeviceState", deviceState);
            cmd.Parameters.AddWithValue("Gmid", gmid);
            cmd.Parameters.AddWithValue("Updated", null);

            return cmd;
        }

        #endregion

        #region MeterSnap Table Handlers

        public SqlCommand MeterSnapInsert(DbHandler db, int snapId, int deviceId, DateTime readDt)
        {
            SqlCommand cmd = db.GetCommand(CommandType.StoredProcedure, "dev.MeterSnap_Insert");

            cmd.Parameters.AddWithValue("SnapId", snapId);
            cmd.Parameters.AddWithValue("DeviceId", deviceId);
            cmd.Parameters.AddWithValue("ReadDt", readDt);

            return cmd;
        }

        #endregion

        #region Metric Table Handlers

        public SqlCommand MetricInsert(DbHandler db, int snapId, int metricCode, long metricValue)
        {
            SqlCommand cmd = db.GetCommand(CommandType.StoredProcedure, "dev.Metric_Insert");

            cmd.Parameters.AddWithValue("SnapId", snapId);
            cmd.Parameters.AddWithValue("MetricCode", metricCode);
            cmd.Parameters.AddWithValue("MetricValue", metricValue);

            return cmd;
        }

        #endregion

        #region Meter Table Handlers

        public SqlCommand MeterInsert(DbHandler db, int meterSnapId, int meterCode, string meterName, long meterValue)
        {
            SqlCommand cmd = db.GetCommand(CommandType.StoredProcedure, "dev.Meter_Insert");

            cmd.Parameters.AddWithValue("MeterSnapId", meterSnapId);
            cmd.Parameters.AddWithValue("MeterCode", meterCode);
            cmd.Parameters.AddWithValue("MeterName", meterName);
            cmd.Parameters.AddWithValue("MeterValue", meterValue);

            return cmd;
        }

        #endregion
    }
}
