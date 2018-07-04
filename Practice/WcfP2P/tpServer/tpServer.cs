using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Configuration;

namespace IG.ThirdParty
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class MyService : IQuickReturnTraderChat
    {
        public void Say(string user, string message)
        {
            throw new NotImplementedException();
        }
    }

    class tpServer
    {
        private static System.Threading.AutoResetEvent stopFlag = new System.Threading.AutoResetEvent(false);

        public static void Main()
        {
            ServiceHost svh = new ServiceHost(typeof(MyService));

            NetPeerTcpBinding binding = new NetPeerTcpBinding();
            binding.Name = "BindingUnsecure";
            binding.Security.Mode = SecurityMode.None;
            binding.Resolver.Mode = System.ServiceModel.PeerResolvers.PeerResolverMode.Pnrp;

            svh.AddServiceEndpoint(
                typeof(IQuickReturnTraderChat),
                binding,
                "net.p2p://QuickReturnTraderChat");

            svh.Open();

            Console.WriteLine("SERVER - Running...");
            stopFlag.WaitOne();

            Console.WriteLine("SERVER - Shutting down...");
            svh.Close();

            Console.WriteLine("SERVER - Shut down!");
        }

        public static void Stop()
        {
            stopFlag.Set();
        }
    }
}

