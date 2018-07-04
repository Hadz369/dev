using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;

namespace HomeServer.Core
{
    public class Server
	{
        /// <summary>
        /// Listener thread control variables
        /// </summary>
        BackgroundWorker _bw = new BackgroundWorker();

        dhEventHandler eh = dhEventHandler.Instance;

        public Server()
        {
            _bw.DoWork += new DoWorkEventHandler(DoWork);
        }

        public void Start()
        {
            try
            {
                eh.RaiseEvent(new MessageEventData("The dhHome Server is starting"));
                _bw.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                eh.RaiseEvent(new MessageEventData("Start: " + ex.Message, EventLevel.Error));
            }
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Network net = new Network();
                net.Start();
            }
            catch (Exception ex)
            {
                eh.RaiseEvent(new MessageEventData(ex.Message, EventLevel.Error));
            }
        }

        public bool IsBusy { get { return _bw.IsBusy; } }
    }
}
