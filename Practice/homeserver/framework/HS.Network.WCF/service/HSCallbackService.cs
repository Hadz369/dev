using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using HS;

namespace HS.Network.WCF
{
    /// <summary>
    /// The instance cotext mode does not really make any difference as the callback is 
    /// registered per proxy.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HSCallbackService : IHSCBServiceContract
    {
        CallbackHandler _cbh = CallbackHandler.Instance;

        public HSCallbackService()
        {
            Tracer.Info("Callback service started");
            _cbh.Start();
        }

        public void Register(Guid guid)
        {
            _cbh.Register(guid);
        }

        public void Deregister(Guid guid)
        {
            _cbh.Deregister(guid);
        }
    }
}
