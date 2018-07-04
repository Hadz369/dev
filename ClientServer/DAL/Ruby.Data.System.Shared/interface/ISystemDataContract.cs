using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Data;

namespace Ruby.Data
{
    [ServiceContract()]
    public interface ISystemDataContract
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void InsertSnap(int snapType);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        int InsertAudit(int auditType, string message);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<Code> GetCodes(string typedefn);
    }
}
