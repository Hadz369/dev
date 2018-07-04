using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data;

using Metro;

namespace RestServer
{
    [ServiceContract(Name="Trace Service", Namespace="")]
    public interface ITraceContract
    {
        [OperationContract]
        [WebGet(UriTemplate = "{sessionkey}/test")]
        Response Test(string sessionkey);

        [OperationContract]
        [WebGet(UriTemplate = "/signon/{vendor}/{device}")]
        Response SignOn(string vendor, string device);

        [OperationContract]
        [WebGet(UriTemplate = "/signoff/{sessionkey}")]
        Response SignOff(string sessionkey);

        [OperationContract(Name="GetData")]
        [WebGet(UriTemplate = "{sessionkey}/getdata/{Method}")]
        Response GetData(string sessionkey, string method);

        [OperationContract(Name = "GetDataWithParameter")]
        [WebGet(UriTemplate = "{sessionkey}/getdata/{Method}/{Parms}")]
        Response GetData(string sessionkey, string method, string parms);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{sessionkey}/account", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json)]
        Response UpdateAccount(string sessionkey, object account);

        [OperationContract]
        [WebGet(UriTemplate = "{sessionkey}/MyUserAgent")]
        Response GetMyUserAgent(string sessionkey);

        [OperationContract]
        [WebGet(UriTemplate = "{sessionkey}/blabla")]
        Response GetSessionInfo(string sessionkey);
    }
}
