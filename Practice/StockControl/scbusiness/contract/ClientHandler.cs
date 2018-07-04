using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using System.Threading;
using StockControl.Common;
using StockControl.Core.Data;

namespace StockControl.Core
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ClientHandler : IClientHandler
    {
        StockControlDTO _dto = StockControlDTO.Instance;

        Timer _timer;
        IClientHandlerCallback _callback;

        public ClientHandler()
        {
            _timer = new Timer(new TimerCallback(TimerFired));
        }

        public void Subscribe()
        {
            _callback = OperationContext.Current.GetCallbackChannel<IClientHandlerCallback>();

            try { _callback.OnCallback("Client callback registered"); }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }

            _timer.Change(5000, 5000);
        }

        public void SendMessage(string message)
        {
            if (_callback != null)
            {
                try { _callback.OnCallback(message); }
                catch { }
            }
        }

        private void TimerFired(Object stateInfo)
        {
            SendMessage("Client Timer Fired");
        }


        public void Unsubscribe()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            SendMessage("Client unsubscribed");
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public DataTable GetTypes()
        {
            DataTable dt = _dto.GetTypes();

            return dt;
        }

        public int DoStuff()
        {
            Random rand = new Random();
            int x = rand.Next(100000);

            Worker w = new Worker();

            IClientHandlerCallback callback = OperationContext.Current.GetCallbackChannel<IClientHandlerCallback>();
            Thread thr = new Thread(new ParameterizedThreadStart(w.DoWork));
            thr.Start(new ThreadParms(callback, x));

            return x;
        }
    }

    public class ThreadParms
    {
        public ThreadParms(IClientHandlerCallback callback, int x)
        {
            Callback = callback;
            Counter = x;
        }

        public IClientHandlerCallback Callback { get; private set; }
        public int Counter { get; private set; }
    }

    public class Worker
    {
        public void DoWork(object parms)
        {
            ThreadParms p = parms as ThreadParms;

            Thread.Sleep(1000);
            p.Callback.OnCallback(p.Counter.ToString());
        }
    }
}
