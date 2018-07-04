using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;
using Ruby.Core;

namespace Ruby.Data
{
    public class SystemDataService : DataServiceBase
    {
        public SystemDataService()
            : base("SYSTEM", "System Data Service") { }

        public override void ThreadProc()
        {
            using (ServiceHost sh = new ServiceHost(typeof(SystemDataContract),
                new Uri[]{
                    new Uri("net.pipe://localhost"),
                    new Uri("net.tcp://localhost:18761") }))
            {
                sh.AddServiceEndpoint(typeof(ISystemDataContract), new NetTcpBinding(SecurityMode.None), "SystemTCP");
                sh.AddServiceEndpoint(typeof(ISystemDataContract), new NetNamedPipeBinding(), "SystemPipe");

                sh.Open(new TimeSpan(0, 0, 10));

                while (sh.State != CommunicationState.Opened)
                    Thread.Sleep(10);

                _isRunning = true;

                Tracer.Info(base.Name + " service started");

                while (!_stop)
                {
                    Thread.Sleep(10);
                }
            }
        }
    }

}
