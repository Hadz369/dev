using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Data;

namespace Metro.Data
{
    [ServiceContract()]
    public interface ISystemDataContract
    {
        [OperationContract]
        Response InsertSnap(Request request);

        [OperationContract]
        Response InsertAudit(Request request);

        [OperationContract]
        Response CodeHandler(Request request);
    }
}
