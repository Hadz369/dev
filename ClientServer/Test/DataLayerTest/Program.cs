using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using eBet.Core;
using eBet.Data;

namespace DataLayerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.TraceLevel level;
            DbConnectionBroker cmgr = DbConnectionBroker.Instance;
            
            level = (System.Diagnostics.TraceLevel)Enum.Parse(typeof(System.Diagnostics.TraceLevel), Properties.Settings.Default.TraceLevel);

            // Configure the file and console tracers
            try
            {
                string logPath = Path.Combine(new string[] { "eBet", "log", Environment.UserName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name });

                // Log to the common application data folder under the user name.
                logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), logPath);

                if (!Directory.Exists(logPath)) 
                    Directory.CreateDirectory(logPath);

                // Create a daily file trace listener.
                LogTracer _logFileDailyListener = new LogTracer(new TracerFileOptions(
                    logPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)) { TraceLevel = level };
                Tracer.Add(_logFileDailyListener);

                /*
                // Create a daily file trace listener.
                ConsoleTracer _consoleListener = new ConsoleTracer(); // Default to Info
                Tracer.Add(_consoleListener);
                */
            }
            catch (Exception ex)
            {
                string msg = "An error occurred while preparing the trace options";

                Tracer.Error(msg, ex);
                throw new Exception(msg, ex);
            }

            Thread t1 = new Thread(new ThreadStart(Go1));
            Thread t2 = new Thread(new ThreadStart(Go2));
            //Thread t3 = new Thread(new ThreadStart(Go1));
            //Thread t4 = new Thread(new ThreadStart(Go2));
            //Thread t5 = new Thread(new ThreadStart(Go1));
            //Thread t6 = new Thread(new ThreadStart(Go2));

            t1.Start();
            t2.Start();
            //t3.Start();
            //t4.Start();
            //t5.Start();
            //t6.Start();

            while (t1.IsAlive || t2.IsAlive) //|| t3.IsAlive || t4.IsAlive || t5.IsAlive || t6.IsAlive)
            {
                Thread.Sleep(1);
            }

            Tracer.Warning(cmgr.GetSummary());

            Console.WriteLine("Done");
            Console.ReadLine();

            cmgr.Dispose();
        }

        static void Go1()
        {
            for (int x = 0; x < 10000; x++)
            {
                using (DbHandler db = new DbHandler(Properties.Settings.Default.ConnectionString))
                {
                    Tracer.Info("Creating Audit");
                    CoreCommandBuilder cb = new CoreCommandBuilder();
                    db.ExecuteNonQuery(cb.AuditInsert(db, 1, "Test Message"));
                }
            }
        }

        static void Go2()
        {
            for (int x = 0; x < 10000; x++)
            {
                using (DbHandler db = new DbHandler(Properties.Settings.Default.ConnectionString))
                {
                    Tracer.Info("Creating Snap");
                    CoreCommandBuilder cb = new CoreCommandBuilder();
                    db.ExecuteNonQuery(cb.SnapInsert(db, 1));
                }
            }
        }
    }
}