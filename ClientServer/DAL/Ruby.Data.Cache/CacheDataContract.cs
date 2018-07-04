using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using Ruby.Core;

namespace Ruby.Data
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class CacheDataContract : ICacheDataContract
    {
        string _key = "SYSCACHE";

        SystemSqlBuilder _sysSql;

        public CacheDataContract()
        {
            _sysSql = new SystemSqlBuilder();
        }

        public void UpdateCode(Code code)
        {
            Cache.Codes.UpdateCode(code);
        }

        public void RefreshCodes()
        {
            Tracer.Info("Refreshing code cache");
            SystemSqlBuilder sb = new SystemSqlBuilder();
            Exception fx = null;

            using (DbHandler db = new DbHandler(_key))
            {
                try
                {
                    DataTable dt = db.Execute(sb.GetCodes(db, Cache.Codes.LastRefresh));

                    if (dt.Rows.Count > 0)
                    {
                        Tracer.Debug(String.Concat(new string[] { "Adding ", dt.Rows.Count.ToString(), " code records" }));

                        List<Code> codes = new List<Code>();

                        foreach (DataRow row in dt.Rows)
                        {
                            codes.Add(new Code(row));
                        }
                        
                        Cache.Codes.AddCodes(codes);
                        Cache.Codes.SetRefreshed();
                    }
                }
                catch (Exception ex)
                {
                    fx = FaultBuilder.GetGeneralFault(141, "Error refreshing codes", "void RefreshCodes()", ex);
                }
            }

            if (fx != null) throw fx;
        }

        public Code GetCode(string typedefn, string codedefn)
        {
            return Cache.Codes.GetCode(typedefn, codedefn);
        }

        public int GetCodeId(string typedefn, string codedefn)
        {
            return Cache.Codes.GetCodeId(typedefn, codedefn);
        }

        public Code GetCodeByGuid(Guid codeGuid)
        {
            return Cache.Codes.GetCode(codeGuid);
        }

        public Code GetCodeById(int codeId)
        {
            return Cache.Codes.GetCode(codeId);
        }

        public List<Code> GetCodes(string typedefn)
        {
            return Cache.Codes.GetCodes(typedefn);
        }

        public void AddCodes(List<Code> codes)
        {
            Cache.Codes.AddCodes(codes);
        }
    }
}
