using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;

namespace FSW
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost sh = new ServiceHost(typeof(FSWServiceContract)))
            {
                sh.Open(new TimeSpan(0, 0, 10));

                while (sh.State != CommunicationState.Opened)
                    Thread.Sleep(10);
            }

            Console.ReadLine();
        }
    }
}
