using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;

using Metro;

namespace Metro.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class CacheServiceContract : MetroContract
    {
        ChannelBroker<IMetroContract> _broker;

        public CacheServiceContract()
        {
            _broker = ChannelBroker<IMetroContract>.Instance;
        }

        protected override Response Process(Request request)
        {
            Response response = null;

            switch (request.Header.Action)
            {
                case "GetCode":
                    return GetCode(request);
                case "GetCodeId":
                    return GetCodeId(request);
                case "GetCodes":
                    return GetCodes(request);
                case "UpdateCode":
                    return UpdateCode(request);
                case "RefreshCodes":
                    return RefreshCodes(request);
                default:
                    return new Response(
                        request.Header,
                        new FaultData(FaultCode.ParameterError, String.Concat("Invalid cache request: Action=", request.Header.Action)));
            }

            return response;
        }

        private Response UpdateCode(Request request)
        {
            Code c = request.PropertyBag.GetProperty("Code").GetValue<Code>();

            if (c != null)
            {
                Cache.Codes.UpdateCode(c);
                return new Response(request.Header);
            }
            else
            {
                throw new InvalidPropertyException("Missing property 'Code' for UpdateCode method call");
            }
        }

        private Response GetCode(Request request)
        {
            Response response = new Response(request.Header);

            string typeDefn = request.PropertyBag.GetProperty("TypeDefn").GetValue<String>();
            string codeDefn = request.PropertyBag.GetProperty("CodeDefn").GetValue<String>();

            response.Data = Cache.Codes.GetCode(typeDefn, codeDefn);

            if (response.Data == null)
            {
                GetCodesFromDatabase(typeDefn);
                // The data should be in the cache if the last call succeeded and the code exists
                response.Data = Cache.Codes.GetCode(typeDefn, codeDefn);
            }

            return response;
        }

        private Response GetCodeId(Request request)
        {
            Response response = new Response(request.Header);

            string typeDefn = request.PropertyBag.GetProperty("TypeDefn").GetValue<String>();
            string codeDefn = request.PropertyBag.GetProperty("CodeDefn").GetValue<String>();

            response.Data = Cache.Codes.GetCodeId(typeDefn, codeDefn);

            if (response.Data == null)
            {
                GetCodesFromDatabase(typeDefn);
                // The data should be in the cache if the last call succeeded and the code exists
                response.Data = Cache.Codes.GetCodeId(typeDefn, codeDefn);
            }

            return response;
        }

        public Response GetCodes(Request request)
        {
            Response response = new Response(request.Header);

            string typeDefn = request.PropertyBag.GetProperty("TypeDefn").GetValue<String>();

            response.Data = Cache.Codes.GetCodes(typeDefn);

            if (response.Data == null)
            {
                GetCodesFromDatabase(typeDefn);
                // The data should be in the cache if the last call succeeded and the code exists
                response.Data = Cache.Codes.GetCodes(typeDefn);
            }

            return response;
        }

/*
        public Code GetCodeByGuid(Guid codeGuid)
        {
            return Cache.Codes.GetCode(codeGuid);
        }

        public Code GetCodeById(int codeId)
        {
            return Cache.Codes.GetCode(codeId);
        }

        public void AddCodes(List<Code> codes)
        {
            Cache.Codes.AddCodes(codes);
        }
*/
        private Response RefreshCodes(Request request)
        {
            Response response = null;

            /*
            if (!Cache.IsAuthorised("RefreshCodes", request.SessionKey))
            {
                response.ResponseData = new RequestError(RequestErrorCode.Unauthorised, "Unauthorised", null);
                return response;
            }
            */
            Tracer.Info("Refreshing code cache");

            using (ChannelProxy<IMetroContract> proxy = _broker.GetProxy("SYSDATA"))
            {
                try
                {
                    Request req = new Request(request.Header.SessionKey, "Code", "GetCodeChanges");

                    req.PropertyBag.Add(new Property("LastRefresh", Cache.Codes.LastRefresh));

                    response = proxy.Channel.ProcessRequest(req);
                    if (!response.IsFault)
                    {
                        DataTable dt = XmlSerialiser.DataTableFromXml(response.Data.ToString());

                        foreach (DataRow row in dt.Rows)
                        {
                            Cache.Codes.UpdateCode(new Code(row));
                        }
                    }
                }
                catch (CommunicationException ex)
                {
                    response = new Response(request.Header);
                    response.Data = new FaultData(FaultCode.CommunicationError, "A communication error occured during the InsertAudit call", ex);
                }
                catch (Exception ex)
                {
                    response = new Response(request.Header);
                    response.Data = new FaultData(FaultCode.UnmanagedError, "An unmanaged error occured during the InsertAudit call", ex);
                }
            }

            return response;
        }

        private void GetCodesFromDatabase(string typedefn)
        {
            /*
            Response response = null;

            Exception exception = null;

            using (ChannelProxy<ISystemDataContract> proxy = _sysbroker.GetProxy(ChannelFactoryKey.SystemData_NetPipe))
            {
                try
                {
                    Request sr = new Request(new MessageHeader(0, 0), new RequestData("GetGodes"));
                    sr.RequestData.RequestParameters.Add(new RequestParameter("TypeDefn", typedefn));

                    response = proxy.Channel.CodeHandler(sr);
                    if (!response.IsFaulted)
                    {
                        DataTable dt = XmlSerialiser.DataTableFromXml(sr.RequestData.ToString());
                        foreach (DataRow r in dt.Rows) Cache.Codes.UpdateCode(new Code(r));
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            if (exception != null) throw exception;
            */
        }
    }
}
