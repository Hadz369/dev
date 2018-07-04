using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Alma.Module
{
    [ServiceContract]
    public interface IModuleManager 
    {
        [OperationContract]
        bool Register(Alma.Shared.IModule Module);
    }

    class ModuleManager : IModuleManager, IDisposable
    {
        bool _isRunning = false;
        Thread _ModuleManagerThread;


        public ModuleManager()
        {
            _ModuleManagerThread = new Thread(new ThreadStart(ModuleManagerThreadProc));
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
            using (ServiceHost host = new ServiceHost(typeof(ModuleManager),
                new Uri[]{
                new Uri("http://localhost:8000"),
                new Uri("net.pipe://localhost")}));

            _isRunning = true;

            while (_isRunning) 
            { 
                Thread.Sleep(100); 
            }
        }

        public bool Register(Shared.IModule Module)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _ModuleManagerThread = null;
        }
    }
}
