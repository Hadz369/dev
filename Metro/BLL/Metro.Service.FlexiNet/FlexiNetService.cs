using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;

using Metro;

namespace Metro.Service
{
    public class FlexiNetService : BusinessServiceBase
    {
        public FlexiNetService()
            : base("FLEXINET", "FlexiNet") { }

        public override void ThreadProc()
        {
            using (ServiceHost sh = new ServiceHost(typeof(FlexiNetServiceContract),
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

                sh.AddServiceEndpoint(typeof(IFlexiNetServiceContract), tcpBinding, "FlexiNet");
                sh.AddServiceEndpoint(typeof(IFlexiNetServiceContract), npBinding, "FlexiNet");

                ChannelBroker<IMetroContract> cb = ChannelBroker<IMetroContract>.Instance;

                // Register the channel to the flexinet data service
                cb.Register("FLEXDATA", new ChannelFactory<IMetroContract>(
                    tcpBinding,                    
                    new EndpointAddress("net.tcp://localhost:18760/FlexiNet")));

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
