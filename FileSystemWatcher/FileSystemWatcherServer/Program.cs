using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using System.Diagnostics;
using System.ServiceProcess;

namespace FSW
{
    class Program
    {
        static Thread _fswThread;

        static void Main(string[] args)
        {
            try
            {
                Log.Initialise("FSWServer", false);
                Log.ConfigureDefaultTrace(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                FSWService fsw = new FSWService();

                if (Environment.UserInteractive)
                {
                    _fswThread = new Thread(new ThreadStart(fsw.Start));
                    _fswThread.Start();

                    ServiceHost sh = new ServiceHost(typeof(FSWServiceContract));

                    sh.Open(new TimeSpan(0, 0, 10));

                    while (sh.State != CommunicationState.Opened)
                        Thread.Sleep(10);

                    Log.Debug("Press any key to exit");
                    Console.ReadLine();

                    fsw.Stop();
                    _fswThread.Join(3000);
                }
                else
                {
                    var servicesToRun = new ServiceBase[] { new Service1() };
                    ServiceBase.Run(servicesToRun);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} -> {1} -> {2}", ex.GetType(), ex.Message, ex.StackTrace);
            }
        }

        public partial class Service1 : ServiceBase
        {
            public Service1() { }
            public void Start(string[] args) { OnStart(args); }

            protected override void OnStart(string[] args) { }
            protected override void OnStop() { }
        }
    }
}
