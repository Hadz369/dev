using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Threading;
using System.IO;
using System.Data;
using Ruby.Core;

namespace Ruby.Data
{
    class Program
    {
        static void Main(string[] args)
        {
            Tracer.CreateConsoleListener(System.Diagnostics.TraceLevel.Info);

            Tracer.CreateLogFileListener(
                "Ruby",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Diagnostics.TraceLevel.Verbose,
                false);

            // Register the channel brokers
            ChannelBroker<ICacheDataContract> cacheBroker = ChannelBroker<ICacheDataContract>.Instance;
            ChannelBroker<ISystemDataContract> systemBroker = ChannelBroker<ISystemDataContract>.Instance;
            
            cacheBroker.Register(ChannelFactoryKey.Cache_NetPipe, new ChannelFactory<ICacheDataContract>(
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/CachePipe")));

            systemBroker.Register(ChannelFactoryKey.System_NetPipe, new ChannelFactory<ISystemDataContract>(
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/SystemPipe")));
            
            systemBroker.Register(ChannelFactoryKey.System_NetTcp, new ChannelFactory<ISystemDataContract>(
                new NetTcpBinding(SecurityMode.None),
                new EndpointAddress("net.tcp://localhost:18761/SystemTcp")));

            using (ChannelProxy<ICacheDataContract> proxy = cacheBroker.GetProxy(ChannelFactoryKey.Cache_NetPipe))
            {
                try
                {
                    proxy.Channel.RefreshCodes();
                }
                catch (Exception ex)
                {
                    Tracer.Error("Error refreshing codes", ex);
                }
            }

            using (ChannelProxy<ISystemDataContract> proxy = systemBroker.GetProxy(ChannelFactoryKey.System_NetPipe))
            {
                try
                {
                    List<Code> codes = proxy.Channel.GetCodes("");

                    if (codes != null)
                    {
                        /*
                        string xml = XmlSerialiser.DataTableToXml(dt);
                        Tracer.Debug(String.Concat("Formatted code data:\n", xml));
                        DataTable newdt = XmlSerialiser.DataTableFromXml(xml);
                        */
                        Tracer.Info(String.Concat(codes.Count, " rows read"));
                    }

                    for (int x = 1; x <= 10; x++)
                    {
                        int id = proxy.Channel.InsertAudit(x, String.Concat("Message: Code=", x));
                        Tracer.Debug(String.Concat("Audit record inserted: Id=", id));
                    }
                }
                catch (FaultException ex)
                {
                    Tracer.Error("Error doing things of importance: FaultCode=" + ex.Code, ex);
                }
            }

            Console.WriteLine("Press almost any key to close");
            Console.ReadLine();

            ChannelProxy<ISystemDataContract> sysTcp = null;
            try
            {
                sysTcp = systemBroker.GetProxy(ChannelFactoryKey.System_NetTcp);
                sysTcp.Channel.InsertAudit(0, String.Concat("Service Stopped"));
            }
            catch (Exception ex)
            {
                Tracer.Error("Something bad happened with the sysTcp channel proxy.", ex);
            }

            if (sysTcp != null) sysTcp.Dispose();

            cacheBroker.Dispose();
            systemBroker.Dispose();
        }
    }
}
