using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Threading;

namespace Alma.Business
{
    /*
    public static class Server
    {
        static Security _security = null;
        static Member _member = null;

        public static void Initialise()
        {
            _security = new Security();
            _member = new Member();
        }

        public static Security Security { get { return _security; } }
    }
    */
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SecurityService : ISecurityContract
    {
        public SecurityService()
        {
        }

        public List<string> GetUserRights()
        {
            return new List<string> { "A", "B", "C" };
        }
    }

    public class Server
    {
        Thread _thread;
        bool _isRunning = false;

        public Server()
        {
            _thread = new Thread(new ThreadStart(ThreadProc));
        }
     
        public void Start()
        {
            if (_thread.ThreadState != ThreadState.Running)
            {
                _thread.Start();
            }
        }

        public void Stop()
        {
            _isRunning = false;

            _thread.Join(5000);

            if (_thread.ThreadState != ThreadState.Stopped)
                _thread.Abort();
        }

        void ThreadProc()
        {
            using (ServiceHost host = new ServiceHost(typeof(SecurityService),
                new Uri[]{
                new Uri("http://localhost:8766"),
                new Uri("net.pipe://securityservice")}))
            {
                _isRunning = true;

                while (_isRunning)
                {
                    Thread.Sleep(1);
                }
            }
        }

        public void Dispose()
        {
            _thread = null;
        }
    }
}
