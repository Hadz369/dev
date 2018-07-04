using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.ServiceModel;
using System.Threading;
using Ruby.Core;

namespace Ruby.Data
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class MemberDataContract : IMemberDataContract
    {
        string _key = "SYSTEM";
        SystemSqlBuilder _sysCmd = new SystemSqlBuilder();

        public void InsertSnap(int snapType)
        {
            using (DbHandler db = new DbHandler(_key))
            {
                db.ExecuteNonQuery(_sysCmd.SnapInsert(db, snapType));
            }
        }

        public int InsertAudit(int auditType, string message)
        {
            int id = 0;

            using (DbHandler db = new DbHandler(_key))
            {
                object o = db.ExecuteNonQuery(_sysCmd.AuditInsert(db, auditType, message));
                id = Convert.ToInt32(o);
            }

            return id;
        }

        public System.Data.DataTable GetCodes(string typedefn)
        {
            Tracer.Info(String.Concat("Reading codes: Type=", typedefn));

            DataTable dt;

            using (DbHandler db = new DbHandler(_key))
            {
                dt = db.Execute(_sysCmd.GetCodes(db, typedefn));
                dt.TableName = "Codes"; //Required for WCF
            }

            return dt;
        }
    }
}
