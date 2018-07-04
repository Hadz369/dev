using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Threading;

using Metro;

namespace Metro.Service
{
    public class CacheService : BusinessServiceBase
    {
        public CacheService() : 
            base("CACHE", "Cache") { }

        public override void ThreadProc()
        {
            using (ServiceHost sh = new ServiceHost(typeof(CacheServiceContract),
                new Uri[]{
                    new Uri("net.pipe://localhost/bizserver"),
                    new Uri("net.tcp://localhost:18761")}))
            {
                NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                tcpBinding.MaxBufferPoolSize = 20000000;
                tcpBinding.MaxBufferSize = 20000000;
                tcpBinding.MaxReceivedMessageSize = 20000000;

                NetNamedPipeBinding npBinding = new NetNamedPipeBinding();
                npBinding.MaxBufferPoolSize = 20000000;
                npBinding.MaxBufferSize = 20000000;
                npBinding.MaxReceivedMessageSize = 20000000;

                sh.AddServiceEndpoint(typeof(IMetroContract), tcpBinding, "Cache");
                sh.AddServiceEndpoint(typeof(IMetroContract), npBinding,  "Cache");

                // Register the channel to the system data service
                ChannelBroker<IMetroContract> cb = ChannelBroker<IMetroContract>.Instance;
                cb.Register("SYSDATA", new ChannelFactory<IMetroContract>(
                    npBinding,
                    new EndpointAddress("net.pipe://localhost/dataserver/System")));

                sh.Open(new TimeSpan(0, 0, 10));

                while (sh.State != CommunicationState.Opened)
                    Thread.Sleep(10);

                Tracer.Info(base.Name + " service started");

                _isRunning = true;

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
