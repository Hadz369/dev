using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Threading;

using Metro;

namespace Metro.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class FlexiNetServiceContract : MetroContract, IFlexiNetServiceContract
    {
        ChannelBroker<IMetroContract> _broker;

        public FlexiNetServiceContract()
        {
            _broker = ChannelBroker<IMetroContract>.Instance;
        }

        protected override Response Process(Request request)
        {
            Tracer.Info(string.Format("FlexiNet service request: Handler={0}, Action={1}", request.Header.Handler, request.Header.Action));

            Response response = null;

            switch (request.Header.Handler)
            {
                case "Machine":
                    using (MachineHandler m = new MachineHandler(_broker.GetProxy("FLEXDATA")))
                        response = m.ProcessRequest(request);
                    break;
                case "Member":
                    using (MemberHandler m = new MemberHandler(_broker.GetProxy("FLEXDATA")))
                        response = m.ProcessRequest(request);
                    break;
                default:
                    response = new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid flexinet request handler: ", request.Header.Handler)));
                    break;
            }

            return response;
        }
    }
}