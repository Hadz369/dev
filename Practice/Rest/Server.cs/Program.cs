using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.Threading;
using System.Configuration;

namespace RestServer
{
    class Program
    {
        static WebServiceHost _host;

        static void Main(string[] args)
        {
            Biz.Init(ConfigurationManager.AppSettings["ConString"]);

            _host = new WebServiceHost(typeof(TpService), new Uri("http://localhost:12321"));

            // This is for debugging only. It sends the exception to the client.
            //_host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            //_host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });

            try
            {
                _host.Open();

                Console.WriteLine("Press CTRL-C");

                Console.ReadLine();

                _host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
