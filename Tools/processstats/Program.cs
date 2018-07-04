using System;
using System.Collections.Generic;
using System.Text;
using Coder24.ProcessManager;
using System.Diagnostics;
using System.Threading;

namespace Coder24
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessMonitor proc = new ProcessMonitor("firefox");

            if (proc.IsProcessRunning() != true)
            {
                Console.Write("{0} is not running.", proc.Name);
            }
            else
            {
                proc.Monitor();
            }

            Console.ReadLine();
        }
    }
}

namespace Coder24
{
    namespace ProcessManager
    {
        /// <summary>
        ///
        /// </summary>
        class ProcessMonitor
        {
            bool _isRunning = false;
            private string processname;
            private PerformanceCounter pcProcess, pcTotal;

            /// <summary>
            /// Constructor.
            /// </summary>
            public ProcessMonitor()
            {
            }

            /// <summary>
            /// Constructor, with overloads.
            /// </summary>
            /// <param name="program"></param>
            public ProcessMonitor(string program)
            {
                processname = program;
                Name = processname;
            }

            /// <summary>
            ///
            /// </summary>
            public string Name
            {
                get;
                set;
            }

            /// <summary>
            /// Determines if the process is running or NOT.
            /// </summary>
            public bool IsProcessRunning()
            {
                Process[] proc = Process.GetProcessesByName(processname);
                return !(proc.Length == 0 && proc == null);
            }

            public void Stop()
            {
                _isRunning = false;
            }

            /// <summary>
            /// Monitors the running program.
            /// </summary>
            public void Monitor()
            {
                int intInterval = 500;

                _isRunning = true;
                
                Console.WriteLine("Monitoring {0} for CPU usage...", processname);

                while (_isRunning)
                {
                    Process[] runningNow = Process.GetProcessesByName(processname);

                    foreach (Process process in runningNow)
                    {
                        pcTotal =   new PerformanceCounter("Processor", "% Processor Time", "_Total");
                        pcProcess = new PerformanceCounter("Process",   "% Processor Time", process.ProcessName);
 
                        pcTotal.NextValue();
                        pcProcess.NextValue();

                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine("Total: {0}, {1} CPU%: {2}", pcTotal.NextValue(), processname, pcProcess.NextValue());
                    }

                    Thread.Sleep(intInterval);
                }
            }
        }
    }
}
