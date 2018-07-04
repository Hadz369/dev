using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Runtime.Serialization;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.GateWayServiceClient client = new ServiceReference1.GateWayServiceClient("WS");

            client.ClientCredentials.UserName.UserName = "eps";
            client.ClientCredentials.UserName.Password = "ebet";
            client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

            Console.WriteLine(client.GetTotalMachinesInPlay());

            Console.ReadLine();
        }
    }
}
