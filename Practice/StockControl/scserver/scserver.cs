using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;
using System.Text;
using System.ServiceModel.Description;
using StockControl.Common;
using StockControl.Core;

namespace StockControl
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost mesgHost, dataHost, webHost;

            mesgHost = new ServiceHost(typeof(MessageHandler), new Uri[] { new Uri("net.pipe://localhost"), new Uri("net.tcp://localhost:8000") });
            mesgHost.AddServiceEndpoint(typeof(IMessageHandler), new NetNamedPipeBinding(), "MessageHandler");
            mesgHost.AddServiceEndpoint(typeof(IMessageHandler), new NetTcpBinding(SecurityMode.Transport), "MessageHandler");
            mesgHost.Faulted += new EventHandler(mesgHost_Faulted);
            mesgHost.Open();
            Console.WriteLine("Messenger service is available.");

            dataHost = new ServiceHost(typeof(ClientHandler), new Uri[] { new Uri("net.tcp://localhost:8001") });
            dataHost.AddServiceEndpoint(typeof(IClientHandler), new NetTcpBinding(SecurityMode.Transport), "ClientHandler");
            dataHost.Faulted += new EventHandler(dataHost_Faulted);
            dataHost.Open();
            Console.WriteLine("Data service is available.");

            webHost = new ServiceHost(typeof(WebHandler), new Uri[] { new Uri("http://localhost:8002") });
            webHost.AddServiceEndpoint(typeof(IWebHandler), new BasicHttpBinding(), "WebHandler");
            webHost.Faulted += new EventHandler(webHost_Faulted);

            // Enable metadata publishing for the web host
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            webHost.Description.Behaviors.Add(smb);
            webHost.Open();
                
            Console.WriteLine("Web service is available.");

            Client c = new Client();
            c.StartMessageHandler();
            c.StartClientHandler();

            Console.WriteLine("Press <ENTER> to exit.");
            Console.ReadLine();

            Console.WriteLine("Closing services.");
            c.ClientHandler.Stop(); 
            c.MessageHandler.Stop();

            webHost.Close();
            Console.WriteLine("Web host closed.");

            dataHost.Close();
            Console.WriteLine("Client host closed.");

            mesgHost.Close();
            Console.WriteLine("Message host closed.");

            Console.ReadLine();
        }

        static void webHost_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Web host faulted");
        }

        static void dataHost_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Client host faulted");
        }

        static void mesgHost_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Message host faulted");
        }
    }
}

/*
            using (dataHost = new ServiceHost( 
                typeof(StringReverser),
                new Uri[]{
                    //new Uri("http://localhost:8000"),
                    new Uri("net.pipe://localhost"),
                    new Uri("net.tcp://localhost:8001")
                }))
            {
                //host.AddServiceEndpoint(typeof(IStringReverser), new BasicHttpBinding(), "Reverse");

                host.AddServiceEndpoint(typeof(IStringHandler), new NetNamedPipeBinding(), "PipeReverse");

                host.AddServiceEndpoint(typeof(IStringHandler), new NetTcpBinding(SecurityMode.Transport), "TcpReverse");

                host.Open();

                Console.WriteLine("Service is available. Press <ENTER> to exit.");
                Console.ReadLine();

                host.Close();
            }
*/