using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data;

namespace RestServer
{
    [ServiceContract(Name="TpMemberService", Namespace="")]
    public interface IMemberData
    {
        [OperationContract]
        [WebGet(UriTemplate = "{sessionkey}/test")]
        ServiceResponse Test(string sessionkey);

        [OperationContract]
        [WebGet(UriTemplate = "/signon/{vendor}/{device}")]
        ServiceResponse SignOn(string vendor, string device);

        [OperationContract]
        [WebGet(UriTemplate = "/signoff/{sessionkey}")]
        ServiceResponse SignOff(string sessionkey);

        [OperationContract(Name="GetData")]
        [WebGet(UriTemplate = "{sessionkey}/getdata/{Method}")]
        ServiceResponse GetData(string sessionkey, string method);

        [OperationContract(Name = "GetDataWithParameter")]
        [WebGet(UriTemplate = "{sessionkey}/getdata/{Method}/{Parms}")]
        ServiceResponse GetData(string sessionkey, string method, string parms);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{sessionkey}/account", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json)]
        ServiceResponse UpdateAccount(string sessionkey, AccountTransaction account);

        [OperationContract]
        [WebGet(UriTemplate = "{sessionkey}/MyUserAgent")]
        ServiceResponse GetMyUserAgent(string sessionkey);

        [OperationContract]
        [WebGet(UriTemplate = "{sessionkey}/blabla")]
        ServiceResponse GetSessionInfo(string sessionkey);
    }

    public interface IObjectList 
    {
        int ObjCount { get; }
    }
}
