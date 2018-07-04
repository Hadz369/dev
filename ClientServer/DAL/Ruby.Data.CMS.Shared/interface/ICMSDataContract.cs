using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Ruby.Data
{
    [ServiceContract]
    public interface ICMSDataContract
    {
        [OperationContract]
        void ProcessSlotManMeters(CMSMeterReads reads);

        [OperationContract]
        CMSMeterReadBase GetPreviousMeterRead(CMSMeterSetType settype, int gmid);
    }
}
