using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using eBet.Data;

namespace eBet.Service
{
    public partial class CacheService : ServiceBase
    {
        CacheDataService _svc = new CacheDataService();

        public CacheService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _svc.Start();
        }

        protected override void OnStop()
        {
            _svc.Stop();
        }
    }
}
