using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Threading;
using Ruby.Core;

namespace Ruby.Data
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SystemDataContract : ISystemDataContract
    {
        string _key = "SYSTEM";
        SystemSqlBuilder _sysCmd = new SystemSqlBuilder();
        ChannelBroker<ICacheDataContract> _cache;

        public SystemDataContract()
        {
            _cache = ChannelBroker<ICacheDataContract>.Instance;
        }

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
                try
                {
                    object o = db.ExecuteScalar(_sysCmd.AuditInsert(db, auditType, message));
                    id = Convert.ToInt32(o);
                }
                catch (Exception ex)
                {
                    throw FaultBuilder.GetGeneralFault(
                        121,
                        "Error inserting audit record",
                        String.Format("InsertAudit(AuditType={0}, Message={1})", auditType, message),
                        ex);
                }
            }

            return id;
        }

        private List<Code> GetCodesFromDatabase(string typedefn)
        {
            List<Code> codes = null;
            string methoddata = String.Format("List<Code> GetCodesFromDatabase(typedefn={0})", typedefn);

            DataTable dt;

            using (DbHandler db = new DbHandler(_key))
            {
                try
                {
                    Tracer.Info(String.Concat("Reading codes from database: Type=", typedefn));
                    dt = db.Execute(_sysCmd.GetCodes(db, typedefn));

                    if (dt.Rows.Count > 0)
                    {
                        codes = new List<Code>();
                        foreach (DataRow r in dt.Rows)
                        {
                            codes.Add(new Code(r));
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return codes;
        }

        public List<Code> GetCodes(string typedefn)
        {
            List<Code> codes = null;
            bool cacheFaulted = false;
            Exception fx = null;
            string methoddata = String.Format("DataTable GetCodes(typedefn={0})", typedefn);

            try
            {
                using (ChannelProxy<ICacheDataContract> proxy = _cache.GetProxy(ChannelFactoryKey.Cache_NetPipe))
                {
                    try
                    {
                        Tracer.Info(String.Concat("Reading codes from cache: Type=", typedefn));
                        codes = proxy.Channel.GetCodes(typedefn);
                    }
                    catch (Exception ex)
                    {
                        // Generate the exception which will write to the trace log but don't throw the error
                        fx = FaultBuilder.GetGeneralFault(
                            121,
                            "Error reading codes from cache",
                            methoddata,
                            ex);

                        cacheFaulted = true;
                    }
                }

                if (codes == null)
                {
                    codes = GetCodesFromDatabase(typedefn);

                    // Don't worry about writing to the cache if the previous lookup failed
                    if (!cacheFaulted)
                        AddCodesToCache(codes);
                }
            }
            catch (Exception ex)
            {
                // Throw this exception because the database must have had an error
                throw FaultBuilder.GetGeneralFault(
                    121,
                    "Error reading codes",
                    methoddata,
                    ex);
            }

            return codes;
        }

        private void AddCodesToCache(List<Code> codes)
        {
            Exception fx = null;
            string methoddata = String.Format("void AddCodesToCache(CodeCount={0})", codes.Count);

            using (ChannelProxy<ICacheDataContract> proxy = _cache.GetProxy(ChannelFactoryKey.Cache_NetPipe))
            {
                try
                {
                    Tracer.Info(String.Concat("Adding codes To the cache"));
                    proxy.Channel.AddCodes(codes);
                }
                catch (Exception ex)
                {
                    // Generate the exception which will write to the trace log but don't throw the error
                    fx = FaultBuilder.GetGeneralFault(
                        121,
                        "Error adding codes to cache",
                        methoddata,
                       ex);
                }
            }
        }
    }
}