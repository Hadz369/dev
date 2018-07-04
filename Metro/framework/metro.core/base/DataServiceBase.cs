using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Threading;
using Metro;

namespace Metro
{
    public abstract class DataServiceBase : IDataService, IDisposable
    {
        Thread _serviceThread;
        protected bool _stop = false, _isRunning = false;
        string _name = "";

        private string _key;

        public DataServiceBase(string key, string name)
        {
            _key = key;
            _name = name;

            _serviceThread = new Thread(new ThreadStart(ThreadProc));
        }

        public string Key { get { return _key; } }
        public string Name { get { return _name; } }
        public bool IsRunning { get { return _isRunning; } }

        public void Start(string constring)
        {
            bool start = false;

            if (_serviceThread.ThreadState == ThreadState.Unstarted)
            {
                if (constring != String.Empty)
                {
                    // Register the database connection for this service
                    DbConnectionBroker cb = DbConnectionBroker.Instance;
                    cb.Register(_key, constring);

                    start = true;
                }
                else
                {
                    throw new Exception("No connection string provided to data manager servioce class " + _name);
                }
            }
            else if (_serviceThread.ThreadState != ThreadState.Stopped)
            {
                start = true;
            }
            
            if (start)
            {
                Tracer.Info(String.Concat("Starting service: ", _name));
                _serviceThread.Start();
            }
        }

        public void Stop()
        {
            Tracer.Info(String.Concat("Stopping service: ", _name));
            _stop = true;
            _serviceThread.Join();
        }

        public abstract void ThreadProc();

        public void Dispose()
        {
            DbConnectionBroker cb = DbConnectionBroker.Instance;
            cb.DeRegister(_key);
        }
    }
}
