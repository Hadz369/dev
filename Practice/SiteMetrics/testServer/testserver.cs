using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using fnDataServer;

namespace testServer
{
    class testserver
    {
        static SqlConnection _con;
        static ServiceHost svh;
        static BusinessRuleLayer1 _business;

        static void Main(string[] args)
        {
            _business = BusinessRuleLayer1.Instance;
            
            int port = Properties.Settings.Default.ServicePort;

            _con = new SqlConnection(Properties.Settings.Default.ConString);

            svh = new ServiceHost(typeof(ThirdPartyContract));

            svh.AddServiceEndpoint(
                typeof(IService),
                new NetTcpBinding(),
                "net.tcp://localhost:8000");

            _con.Open();
            svh.Open();

            Console.ReadLine();
        }
    }
}
