using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;

namespace Ruby.Data
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class CMSDataContract : ICMSDataContract
    {
        Guid _guid;
        CMSSqlBuilder _cmsSql = new CMSSqlBuilder();
        SystemSqlBuilder _sysSql = new SystemSqlBuilder();

        public CMSDataContract()
        {
            // This guid must match the one in the data service
            _guid = new Guid("3213A168-39D5-456A-9411-87B8F1E0FF38");
        }

        public CMSMeterReadBase GetPreviousMeterRead(CMSMeterSetType settype, int gmid)
        {
            throw new NotImplementedException();
        }

        public void ProcessSlotManMeters(CMSMeterReads reads)
        {
            throw new NotImplementedException();
        }
    }
}
