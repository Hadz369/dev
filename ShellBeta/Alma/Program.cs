using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Alma.Core;

namespace Alma
{
    /// <summary>
    /// This is the main entry point for the application and it is where all singleton classes 
    /// should be initialised. It is responsible for login and update checking. Once each of these
    /// processes has been completed it will launch the shell configured in the app.config file.
    /// </summary>
    class Program
    {
        static ModuleManager _mm;

        //static LogTracer _logFileRollingListener = null;
        static LogTracer _logFileDailyListener = null;
        //static ConsoleTracer _consoleListener = null;

        [STAThread]
        static void Main(string[] args)
        {
            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Configure the log trace
            ConfigureTrace();

            Tracer.WriteLine(new TracerData("Program Started"));

            // Show the login dialog window first
            Login w = new Login();
            w.WindowStyle = System.Windows.WindowStyle.None;
            w.AllowsTransparency = false;
            w.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            bool? rc = w.ShowDialog();

            // If the user has successfully logged in then continue
            if (rc == true)
            {
                // todo: 
                // The login window should return the following:
                // > List of user and computer rights
                // > Availability of updates based on last update check time
                // > Service endpoints
                // > User messages

                // todo: If updates are available they should be copied here before loading any assemblies

                _mm = ModuleManager.Instance;
                _mm.Start();

                string file = Path.Combine(new string[] { 
                appPath, 
                Properties.Settings.Default.ModulePath, 
                Properties.Settings.Default.ShellFileName });

                ShellBase shell = _mm.LoadShell(file);
                
                if (shell != null)
                {
                    try
                    {
                        ImageBrush myBrush = new ImageBrush();
                        myBrush.ImageSource = new BitmapImage(new Uri(String.Concat("pack://application:,,,", Properties.Settings.Default.WaterMarkImage), UriKind.Absolute));
                        shell.Background = myBrush;
                    }
                    catch (Exception ex)
                    {
                        Tracer.WriteLine(new TracerData("Error loading application background image", ex));
                    }

                    shell.ShowDialog();

                    _mm.Stop();

                    Tracer.WriteLine(new TracerData("Program Closed"));
                }
                else
                {
                    Tracer.WriteLine(new TracerData("The configured application shell is invalid. Contact software support."));
                }
            }
        }

        static void ConfigureTrace()
        {
            System.Diagnostics.TraceLevel level;

            level = (System.Diagnostics.TraceLevel)Properties.Settings.Default.LogTraceLevel;

            try
            {
                string logPath;

                // Only one instance is allowed so log to the common program files folder.
                logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "alma\\log");

                if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

                // Create a rolling file trace listener with a maximum file size. This was just for testing.
                //_logFileRollingListener = new FnTextWriterTraceListener(new FnTraceFileOptions(
                //    logPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, Properties.Settings.Default.MaxLogFileSize)) { TraceLevel = level };
                //FnTrace.Add(_logFileRollingListener);

                // Create a daily file trace listener.
                _logFileDailyListener = new LogTracer(new TracerFileOptions(
                    logPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)) { TraceLevel = level };

                Tracer.Add(_logFileDailyListener);

            }
            catch (Exception ex)
            {
                Tracer.WriteLine(new TracerData("An error occurred while preparing the trace options", ex));
            }
        }
    }
}