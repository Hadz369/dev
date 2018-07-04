using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Metro;

namespace Metro.Service
{
    internal class SessionHandler : IDisposable
    {
        public Response ProcessRequest(Request request)
        {
            switch (request.Header.Action)
            {
                case "SignOn":
                    return SignOn(request);
                case "SignOff":
                    return SignOff(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid business handler action: ", request.Header.Action)));
            }
        }

        private Response SignOn(Request request)
        {
            Response response = new Response(request.Header);

            string vendor = request.PropertyBag.GetProperty("Vendor").GetValue<String>();
            int    device = request.PropertyBag.GetProperty("Device").GetValue<Int32>();

            response.Data = Cache.Sessions.CreateSession(vendor, device);

            if (response.Data == null)
            {
                response.Data = new FaultData(FaultCode.Unauthorised, String.Concat("Vendor or device not authorised"));
            }

            return response;
        }

        private Response SignOff(Request request)
        {
            Response response = new Response(request.Header);

            Cache.Sessions.CloseSession(request.Header.SessionKey);

            response.Data = null;

            return response;
        }

        public void Dispose()
        {
        }
    }
}
