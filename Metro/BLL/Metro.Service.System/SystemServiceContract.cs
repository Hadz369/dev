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
    public class SystemServiceContract : MetroContract, ISystemServiceContract
    {
        const string KEY = "System";

        ChannelBroker<IMetroContract> _broker;

        public SystemServiceContract()
        {
            _broker = ChannelBroker<IMetroContract>.Instance;
        }

        protected override Response Process(Request request)
        {
            Response response = null;

            Tracer.Info(string.Format("{0} service request: Handler={1}, Action={2}", KEY, request.Header.Handler, request.Header.Action));

            switch (request.Header.Handler)
            {
                case "ThirdParty":
                    using (ThirdPartyHandler h = new ThirdPartyHandler()) response = h.ProcessRequest(request);
                    break;
                case "Code":
                    using (CodeHandler h = new CodeHandler()) response = h.ProcessRequest(request);
                        break;
                case "Member":
                        using (MemberHandler h = new MemberHandler()) response = h.ProcessRequest(request);
                        break;
                case "Machine":
                        using (MachineHandler h = new MachineHandler()) response = h.ProcessRequest(request);
                        break;
                case "Session":
                        using (SessionHandler h = new SessionHandler()) response = h.ProcessRequest(request);
                        break;
                default:
                    response = new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid request handler: ", request.Header.Handler)));
                    break;
            }

            return response;
        }
    }
}