using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Threading;
using Ruby.Core;

namespace Ruby.Data
{
    public class CMSDataService : DataServiceBase
    {
        public CMSDataService() : base("CMS", "CMS Data Service") { }

        public override void ThreadProc()
        {
            using (ServiceHost host = new ServiceHost(typeof(ICMSDataContract),
                new Uri[]{
                new Uri("http://localhost:18769"),
                new Uri("net.pipe://cmsdata")}))
            {
                _stop = false;

                while (!_stop)
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
