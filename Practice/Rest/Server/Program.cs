using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using RestServer;


namespace Test
{
    class Program
    {
        static WebServiceHost _host;

        static void Main(string[] args)
        {
            //string constr = ConfigurationManager.AppSettings["ConString"];
            //Biz.Init(constr);

            _host = new WebServiceHost(typeof(TpService), new Uri("http://localhost:8000"));
            _host.Closing += _host_Closing;
            ServiceDebugBehavior sdb = _host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = true;
            sdb.IncludeExceptionDetailInFaults = true;
            _host.Open();

            Console.WriteLine("-----------------------------------------------------"); 
            Console.WriteLine("TpService Started: " + DateTime.Now.ToString());
            Console.WriteLine("-----------------------------------------------------"); 
            Console.ReadLine();

            _host.Close();
        }

        static void _host_Closing(object sender, EventArgs e)
        {
            Biz.Instance.Close();
        }
    }
}
