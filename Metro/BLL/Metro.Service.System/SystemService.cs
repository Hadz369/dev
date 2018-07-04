using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;

using Metro;

namespace Metro.Service
{
    public class SystemDataService : BusinessServiceBase
    {
        public SystemDataService()
            : base("SYSTEM", "System") { }

        public override void ThreadProc()
        {
            using (ServiceHost sh = new ServiceHost(typeof(SystemServiceContract),
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

                sh.AddServiceEndpoint(typeof(ISystemServiceContract), tcpBinding, "System");
                sh.AddServiceEndpoint(typeof(ISystemServiceContract), npBinding,  "System");

                ChannelBroker<IMetroContract> cb = ChannelBroker<IMetroContract>.Instance;

                // Register the channel to the system data service
                cb.Register("SYSDATA", new ChannelFactory<IMetroContract>(
                    npBinding,
                    new EndpointAddress("net.pipe://localhost/dataserver/System")));

                // Register the channel to the system data service
                cb.Register("OnlineData", new ChannelFactory<IMetroContract>(
                    npBinding,
                    new EndpointAddress("net.pipe://localhost/dataserver/Online")));

                // Register the channel to the system data service
                cb.Register("GameData", new ChannelFactory<IMetroContract>(
                    npBinding,
                    new EndpointAddress("net.pipe://localhost/dataserver/Game")));

                sh.Open(new TimeSpan(0, 0, 10));

                while (sh.State != CommunicationState.Opened)
                    Thread.Sleep(10);

                _isRunning = true;

                Tracer.Info("Started - " + base.Name);

                while (!_stop)
                {
                    Thread.Sleep(10);
                }
            }
        }
    }

}
