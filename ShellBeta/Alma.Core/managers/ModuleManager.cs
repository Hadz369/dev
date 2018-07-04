using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Alma.Core
{
    public delegate void MessageEventHandler(string message);

    [ServiceContract]
    public interface IModuleManagerService
    {
        [OperationContract]
        bool Register(IModule Module);

        [OperationContract]
        List<string> GetUserRights();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ModuleManagerService : IModuleManagerService
    {
        ModuleManager _mm;

        public ModuleManagerService()
        {
            _mm = ModuleManager.Instance;
        }

        public bool Register(IModule module)
        {
            throw new NotImplementedException();
        }


        public List<string> GetUserRights()
        {
            throw new NotImplementedException();
        }
    }

    public class ModuleManager
    {
        public event MessageEventHandler MessageEvent;

        bool _isRunning = false;
        Thread _ModuleManagerThread;
        List<IModule> _modules = new List<IModule>();

        #region Singleton Initialisation

        private static ModuleManager _instance = null;

        private static Object initlock = new Object(); 

        private ModuleManager() 
        {
            _ModuleManagerThread = new Thread(new ThreadStart(ModuleManagerThreadProc));
        }

        private static ModuleManager GetInstance()
        {
            if (_instance == null)
            {
                lock (initlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ModuleManager();
                    }
                }
            }
            return _instance;
        }

        public static ModuleManager Instance { get { return GetInstance(); } }

        #endregion

        public List<IModule> Modules { get { return _modules; } }

        private void RaiseMessageEvent(string message)
        {
            if (MessageEvent != null)
                MessageEvent(message);
        }

        /// <summary>
        /// Load the shell from the location specified in the app.config file.
        /// </summary>
        /// <returns></returns>
        public ShellBase LoadShell(string fileName)
        {
            Tracer.WriteLine(new TracerData("Attempting to load shell from file " + fileName));

            ShellBase sb = null;

            if (File.Exists(fileName))
            {
                try
                {
                    // Load the assembly file
                    Assembly shell = Assembly.LoadFile(fileName);

                    Tracer.WriteLine(new TracerData(String.Format("Shell assembly loaded; FullName={0}", shell.FullName)));

                    // Loop through the types in the loaded file
                    Type[] _types = shell.GetTypes();

                    foreach (Type _type in _types)
                    {
                        // If the type inherits from the ShellBase then create the instance
                        if (_type.BaseType == typeof(ShellBase))
                        {
                            sb = (ShellBase)Activator.CreateInstance(_type, new object[] { });

                            this.Register(sb);

                            Tracer.WriteLine(new TracerData("Shell instance created"));
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    TracerData data = new TracerData("Error loading shell", ex);
                    Tracer.WriteLine(data);
                }
            }
            else
            {
                Tracer.WriteLine(new TracerData(System.Diagnostics.TraceLevel.Error, "File not found"));
            }

            return sb;
        }

        /// <summary>
        /// Loop through all of the DLL files in the module sub folder of the base path.
        /// Load each file and Loop through all of the types. When an IModule interface is found
        /// then initialise the module and store it in an array.
        /// </summary>
        /// <returns></returns>
        public int LoadModules()
        {
            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "module");
            string msg = "";

            Tracer.WriteLine(new TracerData(String.Format("Attempting to load modules from folder {0}", path)));

            foreach (string file in Directory.GetFiles(path, "*.dll"))
            {
                Tracer.WriteLine(new TracerData(String.Format("Loading assembly {0}", file)));

                try
                {
                    Assembly _assembly = Assembly.LoadFile(file);

                    msg = String.Format("Assembly loaded; FullName={0}", _assembly.FullName);
                    Tracer.WriteLine(new TracerData(msg));
                    RaiseMessageEvent(msg);
                    Type[] _types = _assembly.GetTypes();

                    foreach (Type _type in _types)
                    {
                        if (_type.GetInterface("IShell") != null)
                        {
                            // The shell should already be loaded so skip it
                            continue;
                        }
                        else if (_type.GetInterface("IModule") != null)
                        {
                            try
                            {
                                IModule module = (IModule)Activator.CreateInstance(_type, new object[] { });

                                if (!_modules.Contains(module))
                                {
                                    _modules.Add(module);
                                    //this.Register(module);
                                }

                                msg = "Instance created. ModuleDefn = " + module.ModuleDefn;
                                Tracer.WriteLine(new TracerData(msg));
                                RaiseMessageEvent(msg);
                            }
                            catch(Exception ex)
                            {
                                Tracer.WriteLine(new TracerData("Instance creation failed.", ex));
                                throw ex;
                            }
                        }
                    }
                }
                catch (MissingMethodException ex)
                {
                    Tracer.WriteLine(new TracerData(String.Format("A MissingMethodException was thrown while loading module from file {0}.", file), ex));
                }
                catch (TargetInvocationException ex)
                {
                    Tracer.WriteLine(new TracerData(String.Format("A TargetInvocationException was thrown while loading module from file {0}.", file), ex));
                }
                catch (Exception ex)
                {
                    Tracer.WriteLine(new TracerData(String.Format("An unhandled exception was thrown while loading module from file {0}.", file), ex));
                }
            }

            return _modules.Count;
        }

        public void Start()
        {
            if (_ModuleManagerThread.ThreadState != ThreadState.Running)
            {
                _ModuleManagerThread.Start();
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }

        void ModuleManagerThreadProc()
        {
            using (ServiceHost host = new ServiceHost(typeof(ModuleManagerService),
                new Uri[]{
                new Uri("http://localhost:8765"),
                new Uri("net.pipe://modulemanager")}))
            {
                _isRunning = true;

                while (_isRunning)
                {
                    Thread.Sleep(1);
                }
            }
        }

        public bool Register(IModule Module)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _ModuleManagerThread = null;
        }
    }
}
