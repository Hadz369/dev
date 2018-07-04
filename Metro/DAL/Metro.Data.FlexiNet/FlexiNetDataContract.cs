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
    public class FlexiNetDataContract : MetroContract
    {
        const string KEY = "FlexiNet";

        protected override Response Process(Request request)
        {
            Response response = null;
            
            Tracer.Info(string.Format("{0} data request: Handler={1}, Action={2}", KEY, request.Header.Handler, request.Header.Action));

            using (DbHandler db = new DbHandler(KEY))
            {
                switch (request.Header.Handler)
                {
                    case "Machine":
                        MachineHandler machine = new MachineHandler(db);
                        response = machine.ProcessRequest(request);
                        break;
                    case "Member":
                        using (MemberHandler m = new MemberHandler(db))
                            response = m.ProcessRequest(request);
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

        public IAsyncResult BeginRequestAsync(Request request, AsyncCallback callback, object asyncState)
        {
            Console.WriteLine("BeginRequestAsync called: Handler={0}, Action={1}", request.Header.Handler, request.Header.Action);

            Response response = ProcessRequest(request);
            
            return new RequestCompletedAsyncResult<Response>(response);
        }

        public Response EndRequestAsync(IAsyncResult r)
        {
            RequestCompletedAsyncResult<Response> result = r as RequestCompletedAsyncResult<Response>;

            Console.WriteLine("EndRequestAsync called: Handler={0}, Action={1}", result.Data.Header.Handler, result.Data.Header.Action);

            return result.Data;
        }
    }
}
