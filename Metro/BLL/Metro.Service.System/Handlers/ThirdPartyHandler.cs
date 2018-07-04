using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Metro;

namespace Metro.Service
{
    internal class ThirdPartyHandler : IDisposable
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
                request.Header.Handler = "POS";
                response = p.Channel.ProcessRequest(request);
            }

            System.Data.DataTable dt = response.Data as System.Data.DataTable;

            if (dt != null)
            {
                PropertyBagCollection bags = new PropertyBagCollection();

                PropertyBag pb = new PropertyBag();

                foreach (DataRow r in dt.Rows)
                {
                    foreach (DataColumn c in dt.Columns)
                    {
                        pb.Add(new Property(c.ColumnName, r[c]));
                    }

                    bags.Add(pb);
                }

                
                response.Data = bags;

                /*
                MemberInfo mi = null;

                foreach (DataRow r in dt.Rows)
                {
                    mi = new MemberInfo(r);
                    break;
                }

                response.Data = mi;
                */
            }

            return response;
        }

        public void Dispose()
        {
        }
    }
}
