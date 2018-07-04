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
    public class MyService : IService
    {
        TP_BusinessHandler _bus = TP_BusinessHandler.Instance;

        private bool ValidateSession(int sessionId)
        {
            return true;
        }

        public TcpResponse SignOn()
        {
            TcpResponse rc;

            try 
            {
                rc = new TcpResponse(0, _bus.GetSession());
            }
            catch (Exception ex)
            {
                rc = new TcpResponse(1, new Exception("Error during sign on", ex));
            }

            return rc;
        }

        public TcpResponse GetSiteName(TcpRequest request)
        {
            if (ValidateSession(request.SessionId))
            {
                try { return new TcpResponse(_bus.GetSiteName()); }
                catch (Exception ex) { return new TcpResponse(1, new Exception("Error reading site name", ex)); }
            }
            else return new TcpResponse(1, "Invalid session detail provided");
        }
    }

    class tpServer
    {
        private static System.Threading.AutoResetEvent stopFlag = new System.Threading.AutoResetEvent(false);

        public static void Main()
        {
            TP_BusinessHandler _bus = TP_BusinessHandler.Instance;
            _bus.Initialise("Data Source=.;Initial Catalog=EasyNet;Integrated Security=True");

            ServiceHost svh = new ServiceHost(typeof(MyService));

            svh.AddServiceEndpoint(
                typeof(IService),
                new NetTcpBinding(),
                "net.tcp://localhost:8000");

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

