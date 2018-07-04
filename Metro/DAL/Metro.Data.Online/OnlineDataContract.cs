using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.ServiceModel;
using System.Threading;
using Metro;

namespace Metro.Data
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class OnlineDataContract : MetroContract
    {
        const string KEY = "Online";

        protected override Response Process(Request request)
        {
            Response response = null;

            Tracer.Info(string.Format("{0} data request: Handler={1}, Action={2}", KEY, request.Header.Handler, request.Header.Action));

            using (DbHandler db = new DbHandler(KEY))
            {
                switch (request.Header.Handler)
                {
                    case "POS":
                        using (POSHandler h = new POSHandler(db)) response = h.ProcessRequest(request);
                        break;
                    default:
                        response = new Response(
                            request.Header,
                            new FaultData(FaultCode.ParameterError, String.Format(
                                "An invalid action error occured during data request procesing: Service={0}, Action={1}",
                                KEY, request.Header.Handler)));
                        break;
                }
            }

            return response;
        }
    }
}