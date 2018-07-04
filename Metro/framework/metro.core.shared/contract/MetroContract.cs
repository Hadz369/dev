using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Metro
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public abstract class MetroContract : IMetroContract
    {
        protected abstract Response Process(Request request);

        public Response ProcessRequest(Request request)
        {
            Response response = null;

            try
            {
                response = Process(request);
            }
            catch (InvalidPropertyException ex)
            {
                response = new Response(request.Header);
                response.Data = new FaultData(FaultCode.ParameterError, "An invalid property error occured  while processing the request", ex);
            }
            catch (CommunicationException ex)
            {
                response = new Response(request.Header);
                response.Data = new FaultData(FaultCode.CommunicationError, "A communication error occured  while processing the request", ex);
            }
            catch (Exception ex)
            {
                response = new Response(request.Header);
                response.Data = new FaultData(FaultCode.UnmanagedError, "An unmanaged error occured  while processing the request", ex);
            }

            return response;
        }

        public IAsyncResult BeginRequestAsync(Request request, AsyncCallback callback, object asyncState)
        {
            Response response = ProcessRequest(request);

            return new RequestCompletedAsyncResult<Response>(response);
        }

        public Response EndRequestAsync(IAsyncResult r)
        {
            RequestCompletedAsyncResult<Response> result = r as RequestCompletedAsyncResult<Response>;

            return result.Data;
        }
    }
}