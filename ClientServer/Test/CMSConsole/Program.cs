using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.IO;
using eBet.Core;
using eBet.Business;
using eBet.Serial;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CMSConsole
{
    public delegate void MessageEventHandler(string message);

    [ServiceContract]
    public interface IModuleManagerService
    {
        [OperationContract]
        bool Register(object Module);

        [OperationContract]
        List<string> GetUserRights();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ModuleManagerService : IModuleManagerService
    {
        public ModuleManagerService()
        {
        }

        public bool Register(object module)
        {
            throw new NotImplementedException();
        }


        public List<string> GetUserRights()
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static CMSPortListener _cms = null;
        static LogTracer _logFileDailyListener = null;
        static ConsoleTracer _consoleListener = null;
        static bool _stopService = false;
        static Thread _serviceThread;

        static void Main(string[] args)
        {
            ConfigureTrace();

            _serviceThread = new Thread(new ThreadStart(MyThreadProc));

            CMSSettings.Initialise(new SerialPortSettings(
                Properties.Settings.Default.PortName,
                (int)Properties.Settings.Default.BaudRate,
                (Parity)Enum.Parse(typeof(Parity), Properties.Settings.Default.Parity),
                (int)Properties.Settings.Default.DataBits,
                (StopBits)Enum.Parse(typeof(StopBits), Properties.Settings.Default.StopBits)
                ), 
                Properties.Settings.Default.CMSConnectionString);

            StartService();

            B1.Initialise(Properties.Settings.Default.SysConnectionString);
            B2.Initialise();

            _cms = new CMSPortListener(CMSSettings.SerialPortSettings);

            Tracer.Info("Starting CMS listener");
            Thread t = new Thread(new ThreadStart(Run));
            t.Start();

            Console.ReadLine();
            _cms.Stop();
            _stopService = true;
        }

        static void ConfigureTrace()
        {
            System.Diagnostics.TraceLevel level;

            level = (System.Diagnostics.TraceLevel)Enum.Parse(typeof(System.Diagnostics.TraceLevel), Properties.Settings.Default.LogTraceLevel);

            try
            {
                string logPath;

                // Only one instance is allowed so log to the common program files folder.
                logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "CMS\\log");

                if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

                // Create a rolling file trace listener with a maximum file size. This was just for testing.
                //_logFileRollingListener = new FnTextWriterTraceListener(new FnTraceFileOptions(
                //    logPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, Properties.Settings.Default.MaxLogFileSize)) { TraceLevel = level };
                //FnTrace.Add(_logFileRollingListener);

                // Create a daily file trace listener.
                _logFileDailyListener = new LogTracer(new TracerFileOptions(
                    logPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)) { TraceLevel = level };

                // Create a daily file trace listener.
                _consoleListener = new ConsoleTracer();

                Tracer.Add(_logFileDailyListener);
                Tracer.Add(_consoleListener);

            }
            catch (Exception ex)
            {
                Tracer.Error("An error occurred while preparing the trace options", ex);
            }
        }

        static void Run()
        {
            _cms.Start();
        }

        static void StartService()
        {
            if (_serviceThread.ThreadState != ThreadState.Running)
            {
                _serviceThread.Start();
            }
        }

        static void Stop()
        {
            _stopService = true;
        }

        static void MyThreadProc()
        {
            using (ServiceHost host = new ServiceHost(typeof(ModuleManagerService),
                new Uri[]{
                new Uri("http://localhost:8765"),
                new Uri("net.pipe://modulemanager")}))
            {
                _stopService = false;

                while (!_stopService)
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
