using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using Ruby.Core;

namespace Ruby.Data
{
    class Program
    {
        static List<ServiceListItem> _services = new List<ServiceListItem>();
 
        static void Main(string[] args)
        {
            List<string> files = new List<string>();
            files.Add("Ruby.Data.Cache.dll");
            files.Add("Ruby.Data.System.dll");

            Tracer.CreateConsoleListener(System.Diagnostics.TraceLevel.Info);

            Tracer.CreateLogFileListener(
                "Ruby",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Diagnostics.TraceLevel.Verbose,
                false);

            Tracer.Info(String.Concat("Starting Data Server: Log Path = ", Tracer.LogFilePath));

            string syscon = Properties.Settings.Default.SysConString;

            foreach (string f in files)
            {
                string file = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, f);

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
                                _services.Add(new ServiceListItem(file, sb));
                                sb.Start(syscon);
                                while (!sb.IsRunning) { System.Threading.Thread.Sleep(1); }
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
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
