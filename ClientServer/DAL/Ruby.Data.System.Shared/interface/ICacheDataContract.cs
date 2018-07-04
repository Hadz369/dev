using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;

namespace Ruby.Data
{
    [ServiceContract()]
    public interface ICacheDataContract
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        Code GetCode(string typedefn, string codedefn);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        Code GetCodeById(int codeId);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        Code GetCodeByGuid(Guid codeGuid);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<Code> GetCodes(string typedefn);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void AddCodes(List<Code> codes);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        Int32 GetCodeId(string typedefn, string codedefn);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void RefreshCodes();

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void UpdateCode(Code code);
    }
}
