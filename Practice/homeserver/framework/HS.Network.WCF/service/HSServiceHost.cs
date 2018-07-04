using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace HS.Network.WCF
{
    public class HSServiceHost : MarshalByRefObject, IDisposable
    {
        List<ServiceListItem> _services = new List<ServiceListItem>();
        NameValueCollection _appSettings;
        ChannelBroker<IHSContract> _cb;
        bool _isRunning = false;
        BusinessServiceType _serviceType;
        string _assemblyName;

        public HSServiceHost(BusinessServiceType serviceType)
        {
            _serviceType = serviceType;
            _assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }

        public void Start()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                _appSettings = ConfigurationManager.AppSettings;

                _cb = ChannelBroker<IHSContract>.Instance;
                //_cb.MessageEvent += _cb_MessageEvent;

                LoadServices();

                if (_services.Count > 0)
                    _isRunning = true;
            }
            catch (IOException ex)
            {
                using (StreamWriter sw = File.AppendText(@"C:\Temp\Dump.txt"))
                {
                    sw.WriteLine(ex.Message);
                }
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string file = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "HomeServer",
                "Logs", 
                String.Concat(_assemblyName, DateTime.Now.ToString(Tracer.EventTimeFormat)));

            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine(String.Format("Sender={0}, Message={1}", sender.GetType(), (e.ExceptionObject as Exception).ToString()));
            }

            Tracer.Error("Unhandled exception in AppDomain", e.ExceptionObject as Exception);
        }

        public void Stop()
        {
            // Work through in reverse to shutdown the services
            for (int x = _services.Count; x > 0; x--)
            {
                ServiceListItem i = _services[x - 1];
                i.Service.Stop();
                i.Service.Dispose();
            }

            _isRunning = false;
        }

        void _cb_MessageEvent(System.Diagnostics.TraceLevel level, string message, Exception ex)
        {
            Tracer.Info(message);
        }

        string GetAppSetting(string key)
        {
            string value = String.Empty;

            foreach (string name in _appSettings.AllKeys)
            {
                Tracer.Debug(String.Concat("AppSetting: Key=", key, ", Name=", name, ", Value=", _appSettings[name]));

                if (String.Compare(name, key, true) == 0)
                {
                    value = _appSettings[name];
                    break;
                }
            }

            return value;
        }

        void LoadServices()
        {
            string services = GetAppSetting(_serviceType.ToString());

            if (services != String.Empty) // Add the data services
            {
                foreach (string service in services.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string file;

                    file = Path.Combine(new string[] { System.AppDomain.CurrentDomain.BaseDirectory, service });

                    if (File.Exists(file))
                    {
                        try
                        {
                            Assembly asm = Assembly.LoadFile(file);

                            BusinessServiceBase sb = null;

                            // Loop through the types in the loaded file
                            Type[] _types = asm.GetTypes();

                            foreach (Type _type in _types)
                            {
                                // If the type inherits from the BusinesServiceBase then create the instance
                                if (_type.BaseType == typeof(BusinessServiceBase))
                                {
                                    sb = (BusinessServiceBase)Activator.CreateInstance(_type, new object[] { });

                                    // Read the service settings
                                    string sectionName = String.Concat(sb.Key, "_SETTINGS");
                                    var section = ConfigurationManager.GetSection(sectionName) as NameValueCollection;

                                    if (section != null)
                                    {
                                        foreach (string key in section.AllKeys)
                                        {
                                            sb.Properties.Add(key, section[key]);
                                            if (key == "CONSTRING")
                                            {
                                                // Register the database connection for this service
                                                DbConnectionBroker.Instance.Register(sb.Key, section[key]);
                                            }
                                        }
                                    }

                                    Tracer.Info(String.Concat("Business service loaded: Key=", sb.Key, ", Name=", sb.Name, ", File=", file));

                                    foreach (string client in sb.RequiredEndpoints)
                                    {
                                        Tracer.Info(String.Concat("Loading client endpoint: Name=", client));
                                        LoadClientEndpoint(client);
                                    }

                                    sb.Start();

                                    _services.Add(new ServiceListItem(file, sb));

                                    while (!sb.IsRunning) { System.Threading.Thread.Sleep(1); }

                                    break;
                                }
                            }
                        }
                        catch (ReflectionTypeLoadException rex)
                        {
                            string _msg = "";

                            foreach (Exception ex in rex.LoaderExceptions) _msg += ex.Message + "\n";

                            _msg = String.Format("An error occurred while loading a plugin: " +
                                "File={0}, Msg={1}\n\nLoaderExceptions...\n{2}",
                                file, rex.Message, _msg);

                            Tracer.Error(_msg);
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
            }
            else
            {
                Tracer.Info("Service list is empty");
            }
        }

        void LoadClientEndpoint(string name)
        {
            ClientSection clientSection = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

            ChannelEndpointElementCollection endpointCollection = clientSection.ElementInformation.Properties[string.Empty].Value as ChannelEndpointElementCollection;

            List<string> endpointNames = new List<string>();

            foreach (ChannelEndpointElement endpointElement in endpointCollection)
            {
                if (String.Compare(name, endpointElement.Name) == 0)
                {
                    _cb.Register(endpointElement.Name, new System.ServiceModel.DuplexChannelFactory<IHSContract>(endpointElement.Name));
                    break;
                }
            }
        }

        public void Dispose()
        {
            Tracer.Debug("HomeServerServiceHost disposing");

            // For some reason the service is stopping so this will hopefully release the ports.
            if (_isRunning)
            {
                Stop();
            }
        }
    }

    class ServiceListItem
    {
        string _file;
        BusinessServiceBase _sb;

        public ServiceListItem(string fileName, BusinessServiceBase bizService)
        {
            _file = fileName;
            _sb = bizService;
        }

        public string FileName { get { return _file; } }
        public BusinessServiceBase Service { get { return _sb; } }
    }
}
