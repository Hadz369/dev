using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace HS.Network.WCF
{
    enum MessageType
    {
        Poll = 0,
        Meter = 1
    }

    /// <summary>
    /// This is the callback contract
    /// </summary>
    public interface IHSCallback
    {
        [OperationContract(IsOneWay = true)]
        void Callback(string data);
    }

    /// <summary>
    /// The service contract implements the callback contract
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IHSCallback))]
    public interface IHSCBServiceContract
    {
        [OperationContract(IsOneWay = true)]
        void Register(Guid guid);

        [OperationContract(IsOneWay = true)]
        void Deregister(Guid guid);
    }
}
