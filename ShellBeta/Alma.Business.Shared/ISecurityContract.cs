using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Alma.Business
{
    [ServiceContract]
    public interface ISecurityContract
    {
        [OperationContract]
        List<string> GetUserRights();
    }
}
