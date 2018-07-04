using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using Metro;

namespace Metro.Data
{
    class Program
    {
        static List<ServiceListItem> _services = new List<ServiceListItem>();
 
        static void Main(string[] args)
        {
            List<string> files = new List<string>();
            files.Add("Metro.Data.System.dll");
            files.Add("Metro.Data.FlexiNet.dll");
            files.Add("Metro.Data.Online.dll");
            files.Add("Metro.Data.Member.dll");
            files.Add("Metro.Data.Game.dll");

            Tracer.CreateConsoleListener(System.Diagnostics.TraceLevel.Info);

            Tracer.CreateLogFileListener(
                "Metro",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Diagnostics.TraceLevel.Verbose,
                false);

            Tracer.Info(String.Concat("Starting Metro.DataServer: LogPath = ", Tracer.LogFilePath));

            foreach (string f in files)
            {
                string file = Path.Combine(new string[] { System.AppDomain.CurrentDomain.BaseDirectory, "DAL", f });

                if (File.Exists(file))
                {
                    try
                    {
                        // Load the assembly file
                        Tracer.Info("Loading service file: Name=" + file);
                        Assembly assembly = Assembly.LoadFile(file);

                        // Loop through the types in the loaded file
                        Type[] _types = assembly.GetTypes();

                        foreach (Type _type in _types)
                        {
                            // If the type inherits from the DataServiceBase then create the instance
                            if (_type.BaseType == typeof(DataServiceBase))
                            {
                                DataServiceBase sb = (DataServiceBase)Activator.CreateInstance(_type, new object[] { });
                                sb.Start(Properties.Settings.Default[String.Concat(sb.Key, "Connection")].ToString());

                                _services.Add(new ServiceListItem(file, sb));

                                while (!sb.IsRunning) { System.Threading.Thread.Sleep(1); }
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Reflection.ReflectionTypeLoadException)
                        {
                            var typeLoadException = ex as ReflectionTypeLoadException;
                            var loaderExceptions = typeLoadException.LoaderExceptions;
                        }
                        Tracer.Error("Error loading service", ex);
                    }
                }
                else
                {
                    Tracer.Error("Service file not found: Name=" + file);
                }
            }

            Console.ReadLine();

            // Work through in reverse to shutdown the services
            for (int x = _services.Count; x > 0; x-- )
            {
                ServiceListItem i = _services[x - 1];
                i.DataService.Stop();
                i.DataService.Dispose();
            }
        }
    }

    class ServiceListItem
    {
        string _file;
        DataServiceBase _sb;

        public ServiceListItem(string fileName, DataServiceBase dataService)
        {
            _file = fileName;
            _sb = dataService;
        }

        public string FileName { get { return _file; } }
        public DataServiceBase DataService { get { return _sb; } }
    }
}
