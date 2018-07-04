using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Metro;

namespace Metro.Service
{
    internal class MemberHandler : IDisposable
    {
        ChannelBroker<IMetroContract> _broker = ChannelBroker<IMetroContract>.Instance;

        public Response ProcessRequest(Request request)
        {
            switch (request.Header.Action)
            {
                case "GetMemberInfo":
                    return GetMemberInfo(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid business handler action: ", request.Header.Action)));
            }
        }

        private Response GetMemberInfo(Request request)
        {
            Response response = null;

            using (ChannelProxy<IMetroContract> p = _broker.GetProxy("OnlineData"))
            {
                response = p.Channel.ProcessRequest(request);
            }

            return response;
        }

        public void Dispose()
        {
        }
    }
}
