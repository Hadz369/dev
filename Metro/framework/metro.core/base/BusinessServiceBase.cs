using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Threading;
using Metro;

namespace Metro
{
    public abstract class BusinessServiceBase : IBusinessService, IDisposable
    {
        Thread _serviceThread;
        protected bool _stop = false, _isRunning = false;
        string _name = "";

        private string _key;

        public BusinessServiceBase(string key, string name)
        {
            _key = key;
            _name = name;

            _serviceThread = new Thread(new ThreadStart(ThreadProc));
        }

        public string Key { get { return _key; } }
        public string Name { get { return _name; } }
        public bool IsRunning { get { return _isRunning; } }

        public void Start()
        {
            bool start = false;

            if (_serviceThread.ThreadState == ThreadState.Unstarted)
            {
                start = true;
            }
            else if (_serviceThread.ThreadState != ThreadState.Stopped)
            {
                start = true;
            }
            
            if (start)
            {
                Tracer.Info(String.Concat("Starting business service: ", _name));
                _serviceThread.Start();
            }
        }

        public void Stop()
        {
            Tracer.Info(String.Concat("Stopping business service: ", _name));
            _stop = true;
            _serviceThread.Join();
        }

        public abstract void ThreadProc();

        public void Dispose()
        {
        }
    }
}
