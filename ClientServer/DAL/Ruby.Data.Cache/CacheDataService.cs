using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Threading;
using Ruby.Core;

namespace Ruby.Data
{
    public class CacheDataService : DataServiceBase
    {
        public CacheDataService() : 
            base("SYSCACHE", "System Cache Service") { }

        public override void ThreadProc()
        {
            using (ServiceHost sh = new ServiceHost(typeof(CacheDataContract),
                new Uri[]{
                new Uri("net.tcp://localhost:18760"),
                new Uri("net.pipe://localhost")}))
            {
                sh.AddServiceEndpoint(typeof(ICacheDataContract), new NetTcpBinding(), "CacheTCP");
                sh.AddServiceEndpoint(typeof(ICacheDataContract), new NetNamedPipeBinding(), "CachePipe");

                sh.Open(new TimeSpan(0, 0, 10));

                while (sh.State != CommunicationState.Opened)
                    Thread.Sleep(10);

                _isRunning = true;

                Tracer.Info(base.Name + " service started");

                while (!_stop)
                {
                    Thread.Sleep(10);
                }

                sh.Close();
            }
        }

        void OnMessageEvent(string message)
        {
            Tracer.Info(message);
        }
    }
}
