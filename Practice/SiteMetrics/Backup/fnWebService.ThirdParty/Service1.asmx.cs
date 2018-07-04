using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ServiceModel;
using System.Text;
using fnDataServer;

namespace fnWebService.ThirdParty
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        ChannelFactory<IService> scf;

        public Service1()
        {
            scf = new ChannelFactory<IService>(new NetTcpBinding(), String.Format("net.tcp://{0}:8000", "localhost"));
        }

        [WebMethod]
        public string simpleMethod(String srt)
        {
            using (IService s = scf.CreateChannel())
            {
                return "Hello " + srt;
            }
        }

        [WebMethod]
        public int anotherSimpleMethod(int firstNum, int secondNum)
        {
            return firstNum + secondNum;
        }
    }
}