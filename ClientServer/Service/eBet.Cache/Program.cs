﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using eBet.Data;

namespace eBet.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "console")
            {
                CacheDataService s = new CacheDataService();
                s.Start();

                Console.WriteLine();
                s.Stop();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new CacheService() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
