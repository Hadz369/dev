using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Threading;

namespace HS
{
    public enum BusinessServiceType { APP, BLL, DAL }

    public abstract class BusinessServiceBase : IBusinessService, IDisposable
    {
        Thread _serviceThread;
        protected bool _stop = false, _isRunning = false;
        string _name, _key;
        //int _port = 0;
        protected BusinessServiceType _svcType;
        PropertyBag _properties;
        List<string> _requiredEndpoints = new List<string>();

        public BusinessServiceBase(BusinessServiceType svcType, string key, string name)
        {
            _svcType = svcType;
            _key = key;
            _name = name;
            _properties = new PropertyBag();
            _serviceThread = new Thread(new ThreadStart(ThreadProc));
        }

        public BusinessServiceType ServiceType { get { return _svcType; } }
        public string Key { get { return _key; } }
        public string Name { get { return _name; } }
        //public int Port { get { return _port; } set { _port = value; } }
        public bool IsRunning { get { return _isRunning; } }
        public PropertyBag Properties { get { return _properties; } }
        public List<string> RequiredEndpoints { get { return _requiredEndpoints; } }

        public void Start()
        {
            bool start = false;

            if (_svcType == BusinessServiceType.DAL)
            {
                string constring = String.Empty;

                foreach (Property p in _properties)
                {
                    switch (p.Name)
                    {
                        case "CONSTRING":
                            constring = p.Value.ToString();
                            if (constring != String.Empty)
                            {
                                // Register the database connection for this service
                                DbConnectionBroker.Instance.Register(_key, constring);
                                start = true;
                            }
                            else
                            {
                                throw new Exception("No connection string provided to data service class " + _name);
                            }
                            break;
                    }
                }
            }

            if (_serviceThread.ThreadState == ThreadState.Unstarted || _serviceThread.ThreadState == ThreadState.Stopped)
            {
                start = true;
            }

            if (start)
            {
                _serviceThread.Start();

                while (!_serviceThread.IsAlive) Thread.Sleep(1);
            }
        }

        public void Stop()
        {
            Tracer.Always(String.Concat("Stopping service: ", _name));
            _stop = true;
            _serviceThread.Join();
        }

        public abstract void ThreadProc();

        public abstract void Dispose();
    }
}
