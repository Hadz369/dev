using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Ruby.Data
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

        #region Snap Table

        public SqlCommand SnapInsert(DbHandler db, int snapType)
        {
            return SnapInsert(db, snapType, DateTime.Now);
        }

        public SqlCommand SnapInsert(DbHandler db, int snapType, DateTime snapDT)
        {
            if (snapDT == null) snapDT = DateTime.Now;

            SqlCommand cmd = db.GetCommand(CommandType.StoredProcedure, "dev.Snap_Insert");
            
            cmd.Parameters.AddWithValue("SnapType", snapType);
            cmd.Parameters.AddWithValue("SnapDT", snapDT);
            
            return cmd;
        }

        public SqlCommand SnapUpdate(DbHandler db, int snapId, int snapType, DateTime snapDt, int snapReason)
        {
            SqlCommand cmd = db.GetCommand(CommandType.StoredProcedure, "dev.Snap_Update");

            cmd.Parameters.AddWithValue("SnapId", snapId);
            cmd.Parameters.AddWithValue("SnapType", snapType);
            cmd.Parameters.AddWithValue("SnapDt", snapDt);
            cmd.Parameters.AddWithValue("SnapReason", snapReason);
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

        #region Audit Table Handlers

        public SqlCommand AuditInsert(DbHandler db, int auditType, string message)
        {
            SqlCommand cmd = db.GetCommand(CommandType.StoredProcedure, "dev.Audit_Insert");

            cmd.Parameters.AddWithValue("AuditType", auditType);
            cmd.Parameters.AddWithValue("Message", message);

            SqlParameter rcParm = new SqlParameter("RC", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
            cmd.Parameters.Add(rcParm);

            return cmd;
        }

        #endregion

        #region Cache Data Handlers

        public SqlCommand GetCodeType(DbHandler db, string typeDefn)
        {
            string sql = "" +
                "select CodeTypeId, CodeTypeDefn, CodeTypeName, CodeTypeDesc " +
                "from dev.CodeType " +
                "where CodeTypeDefn = @CodeTypeDefn";

            SqlCommand cmd = db.GetCommand(CommandType.Text, sql);

            cmd.Parameters.AddWithValue("CodeTypeDefn", typeDefn);

            return cmd;
        }

        public SqlCommand GetCodes(DbHandler db, DateTime lastRefresh)
        {
            string sql = "" +
                "select t.CodeTypeId, t.TypeDefn, c.CodeId, c.CodeDefn, c.CodeName, c.CodeDesc, c.CodeValue, c.Sequence " +
                "from dev.CodeType t " +
                "inner join dev.Code c on t.CodeTypeId = c.CodeTypeId " +
                "where c.updated >= @lastRefresh";

            SqlCommand cmd = db.GetCommand(CommandType.Text, sql);

            cmd.Parameters.AddWithValue("lastRefresh", lastRefresh);

            return cmd;
        }
        
        public SqlCommand GetCodes(DbHandler db, string codeType)
        {
            string sql = "" +
                "select t.CodeTypeId, t.TypeDefn, c.CodeId, c.CodeDefn, c.CodeName, c.CodeDesc, c.CodeValue, c.Sequence " +
                "from dev.CodeType t " +
                "inner join dev.Code c on t.CodeTypeId = c.CodeTypeId";

            if (codeType != String.Empty) sql += "where t.TypeDefn = @CodeType";

            SqlCommand cmd = db.GetCommand(CommandType.Text, sql);

            if (codeType != String.Empty) cmd.Parameters.AddWithValue("CodeType", codeType);

            return cmd;
        }

        #endregion
    }
}
