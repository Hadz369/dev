using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Configuration;

using HS.Network;
using HS.Network.WCF;

namespace HS
{
    class homeserver
    {
        static bool _wcfStop = true;
        static Timer _timer;
        static CallbackHandler _cbh = CallbackHandler.Instance;

        static void Main(string[] args)
        {
            _timer = new Timer(OnTimer);

            try
            {
                _timer.Change(1000, 1000);

                ConfigureTrace(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                Thread wcfThread = new System.Threading.Thread(new ThreadStart(WCFThreadProc));

                Tracer.Info("Starting the network service");
                NetworkHandler net = new NetworkHandler();
                net.Start();

                Tracer.Info("Starting the WCF service thread");
                wcfThread.Start();

                Console.ReadLine();

                Tracer.Info("Stopping the WCF service");
                _wcfStop = true;
                wcfThread.Join();

                Tracer.Info("Stopping the NET service");
                net.Stop();
                net.Dispose();

                Tracer.Info("See you again soon");
            }
            catch (Exception ex)
            {
                Tracer.Error("Error during service execution", ex);
            }
        }

        static void OnTimer(object stateInfo)
        {
            EnergyMeter em = new EnergyMeter(null);
            em.Meter = 32;
            Packet<EnergyMeter> pkt = new Packet<EnergyMeter>(em);
            _cbh.Enque(pkt.ToJSON());
        }

		static void MessageEventHandler(MessageEventData eventData)
		{
			Tracer.Info(eventData.Message);
		}

        public static void WCFThreadProc()
        {
            try
            {
                using (ServiceHost sh = new ServiceHost(typeof(HSCallbackService)))
                {
                    sh.Open(new TimeSpan(0, 0, 10));

                    while (sh.State != CommunicationState.Opened)
                        Thread.Sleep(10);

                    _wcfStop = false;

                    while (!_wcfStop)
                    {
                        Thread.Sleep(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.Error("Error during WCF thread management", ex);
            }
        }

        static void ConfigureTrace(string assemblyName)
        {
            Tracer.Initialise(assemblyName, false);

            System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;

            if (appSettings.AllKeys.Contains("ConsoleTraceLevel"))
                Tracer.CreateConsoleListener(Tracer.GetTraceLevel(appSettings["ConsoleTraceLevel"]));

            if (appSettings.AllKeys.Contains("LogTraceLevel"))
            {
                Tracer.CreateLogFileListener(assemblyName, Tracer.GetTraceLevel(appSettings["LogTraceLevel"]));
            }

            Tracer.Info(String.Concat("Starting ", assemblyName, ": LogPath = ", Tracer.LogFilePath));
        }
    }
}

