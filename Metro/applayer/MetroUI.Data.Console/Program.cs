using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Threading;
using System.IO;
using System.Data;

using Metro;
using Metro.Service;

namespace Metro
{
    class Program
    {
        static void Main(string[] args)
        {
            Tracer.CreateConsoleListener(System.Diagnostics.TraceLevel.Info);

            Tracer.CreateLogFileListener(
                "Metro",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Diagnostics.TraceLevel.Verbose,
                false);

            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
            tcpBinding.MaxBufferPoolSize = 20000000;
            tcpBinding.MaxBufferSize = 20000000;
            tcpBinding.MaxReceivedMessageSize = 20000000;

            NetNamedPipeBinding npBinding = new NetNamedPipeBinding();
            npBinding.MaxBufferPoolSize = 20000000;
            npBinding.MaxBufferSize = 20000000;
            npBinding.MaxReceivedMessageSize = 20000000;

            ChannelBroker<ISystemServiceContract> cb1 = ChannelBroker<ISystemServiceContract>.Instance;
            ChannelBroker<IFlexiNetServiceContract> cb2 = ChannelBroker<IFlexiNetServiceContract>.Instance;

            // Register the system channel
            cb1.Register("System", new ChannelFactory<ISystemServiceContract>(
                npBinding,
                new EndpointAddress("net.pipe://localhost/bizserver/System")));

            // Register the cache channel
            cb2.Register("FlexiNet", new ChannelFactory<IFlexiNetServiceContract>(
                npBinding,
                new EndpointAddress("net.pipe://localhost/bizserver/FlexiNet")));

            Window w = new Window();
            w.ShowDialog();
        }            
    }
}
