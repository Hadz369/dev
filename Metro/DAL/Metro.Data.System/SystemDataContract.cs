using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Threading;
using System.Collections.Specialized;

namespace Metro.Data
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SystemDataContract : MetroContract
    {
        Response response = null;

        protected override Response Process(Request request)
        {
            const string KEY = "System";

            Tracer.Info(string.Format("{0} data request: Handler={1}, Action={2}", KEY, request.Header.Handler, request.Header.Action));

            using (DbHandler db = new DbHandler(KEY))
            {
                switch (request.Header.Handler)
                {
                    case "Audit":
                        AuditHandler audit = new AuditHandler(db);
                        return audit.ProcessRequest(request);
                    case "Code":
                        CodeHandler code = new CodeHandler(db);
                        return code.ProcessRequest(request);
                    case "Snapshot":
                        SnapshotHandler snapshot = new SnapshotHandler(db);
                        return snapshot.ProcessRequest(request);
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