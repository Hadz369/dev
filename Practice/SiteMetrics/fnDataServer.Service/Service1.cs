using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace fnDataServer.Service
{
    public partial class Service1 : ServiceBase
    {
        private static System.Threading.AutoResetEvent stopFlag = new System.Threading.AutoResetEvent(false);
        ServiceHost svh;
        SqlConnection _con;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            int port = Properties.Settings.Default.ServicePort;

            _con = new SqlConnection(Properties.Settings.Default.ConString);

            svh = new ServiceHost(typeof(ThirdPartyContract));

            svh.AddServiceEndpoint(
                typeof(IService),
                new NetTcpBinding(),
                "net.tcp://localhost:8000");

            _con.Open();
            svh.Open();
        }

        protected override void OnStop()
        {
            svh.Close(new TimeSpan(0, 0, 5));
        }
    }
}
