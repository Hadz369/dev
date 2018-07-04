using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MSDNMagazine;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace SelfHosting
{
    class Program
    {
        static void Main(string[] args)
        {
//ServiceHost sh = new ServiceHost(typeof(MSDNMagazineServiceType));
//string baseUri = "http://localhost/MagazineService";
//ServiceEndpoint se = sh.AddServiceEndpoint(typeof(IMSDNMagazineService),
//                                            new WebHttpBinding(),
//                                            baseUri);
//se.Behaviors.Add(new WebHttpBehavior());
string baseUri = "http://localhost/MagazineService";
WebServiceHost sh = new WebServiceHost(typeof(MSDNMagazineServiceType),
                                    new Uri(baseUri));
sh.Open();
            Console.ReadLine();
        }
    }
}
