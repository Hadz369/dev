using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;
using Metro;

namespace Metro.Data
{
    public class GameDataService : DataServiceBase
    {
        const string KEY = "Game";

        public GameDataService() : base(KEY, String.Concat(KEY, " Data Service")) { }

        public override void ThreadProc()
        {
            using (ServiceHost sh = new ServiceHost(typeof(GameDataContract),
                new Uri[]{
                    new Uri("net.pipe://localhost/dataserver"),
                    new Uri("net.tcp://localhost:18760") }))
            {
                NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                tcpBinding.MaxBufferPoolSize = 20000000;
                tcpBinding.MaxBufferSize = 20000000;
                tcpBinding.MaxReceivedMessageSize = 20000000;

                NetNamedPipeBinding npBinding = new NetNamedPipeBinding();
                npBinding.MaxBufferPoolSize = 20000000;
                npBinding.MaxBufferSize = 20000000;
                npBinding.MaxReceivedMessageSize = 20000000;

                sh.AddServiceEndpoint(typeof(IMetroContract), tcpBinding, KEY);
                sh.AddServiceEndpoint(typeof(IMetroContract), npBinding,  KEY);

                sh.Open(new TimeSpan(0, 0, 10));

                while (sh.State != CommunicationState.Opened)
                    Thread.Sleep(10);

                _isRunning = true;

                Tracer.Info("Started: " + base.Name);

                while (!_stop)
                {
                    Thread.Sleep(10);
                }
            }
        }
    }
}
