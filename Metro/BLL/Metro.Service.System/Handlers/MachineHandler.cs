using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Metro;

namespace Metro.Service
{
    internal class MachineHandler : IDisposable
    {
        ChannelBroker<IMetroContract> _broker = ChannelBroker<IMetroContract>.Instance;

        public Response ProcessRequest(Request request)
        {
            switch (request.Header.Action)
            {
                case "GetMachineDetails":
                    return GetMachineDetails(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid business handler action: ", request.Header.Action)));
            }
        }

        private Response GetMachineDetails(Request request)
        {
            Response response = null;

            using (ChannelProxy<IMetroContract> p = _broker.GetProxy("GameData"))
            {
                response = p.Channel.ProcessRequest(request);
            }

            DataTable dt = response.Data as DataTable;
            if (dt != null)
            {
                MachineDetailCollection dets = new MachineDetailCollection();

                foreach (DataRow r in dt.Rows)
                {
                    dets.Add(new MachineDetail(new DataRowAdapter(r)));
                }

                response.Data = dets;
            }

            return response;
        }

        public void Dispose()
        {
        }
    }
}
