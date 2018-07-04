using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using Metro;

namespace Metro.Service
{
    public class MachineHandler : IDisposable
    {
        ChannelProxy<IMetroContract> _proxy = null;

        public MachineHandler(ChannelProxy<IMetroContract> proxy)
        {
            _proxy = proxy;
        }

        public Response ProcessRequest(Request request)
        {
            Response response = null;

            try
            {
                switch (request.Header.Action)
                {
                    case "GetMachineList":
                        response = _proxy.Channel.ProcessRequest(request);
                        break;
                    default:
                        response = new Response(
                            request.Header,
                            new FaultData(FaultCode.ParameterError, String.Concat("Invalid flexinet request action: ", request.Header.Action)));
                        break;
                }
            }
            catch (CommunicationException ex)
            {
                response = new Response(request.Header);
                response.Data = new FaultData(FaultCode.CommunicationError, "A communication error occured while processing a flexinet request", ex);
            }
            catch (Exception ex)
            {
                response = new Response(request.Header);
                response.Data = new FaultData(FaultCode.UnmanagedError, "An unmanaged error occured while processing a flexinet request", ex);
            }

            return response;
        }


        public void Dispose()
        {
            if (_proxy != null) _proxy.Dispose();
        }
    }
}
