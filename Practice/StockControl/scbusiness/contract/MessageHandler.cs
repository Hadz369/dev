using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using StockControl.Common;

namespace StockControl.Core
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, IncludeExceptionDetailInFaults=true)]
	public class MessageHandler : IMessageHandler
	{
        Timer _timer;

        List<IMessageHandlerCallback> _subscribers = new List<IMessageHandlerCallback>();

        public MessageHandler()
        {
            _timer = new Timer(new TimerCallback(TimerFired));
        }

        public void Subscribe()
        {
            IMessageHandlerCallback callback = OperationContext.Current.GetCallbackChannel<IMessageHandlerCallback>();

            try { callback.OnCallback("Callback registered"); }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }

            lock (_subscribers)
            {
                if (!_subscribers.Contains(callback))
                {
                    _subscribers.Add(callback);
                }
            }

            SetSubscriptionPollTimer();
        }

        private bool HasSubscribers()
        {
            if (_subscribers.Count > 0) return true;
            else return false;
        }

        private void SetSubscriptionPollTimer()
        {
            if (HasSubscribers()) _timer.Change(5000, 5000);
            else _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void SendMessage(string message)
        {
            List<IMessageHandlerCallback> l = null;

            foreach (IMessageHandlerCallback c in _subscribers)
            {
                try
                {
                    c.OnCallback(message);
                }
                catch
                {
                    if (l == null) l = new List<IMessageHandlerCallback>();
                    l.Add(c);
                }
            }

            if (l != null)
            {
                lock (_subscribers)
                {
                    foreach (IMessageHandlerCallback i in l)
                    {
                        try { _subscribers.Remove(i); }
                        catch { }
                    }
                }
            }
        }

        private void TimerFired(Object stateInfo)
        {
            SendMessage("Message Handler Poll");
        }


        public void Unsubscribe()
        {
            IMessageHandlerCallback callback = OperationContext.Current.GetCallbackChannel<IMessageHandlerCallback>();
            if (_subscribers.Contains(callback))
            {
                lock (_subscribers)
                {
                    _subscribers.Remove(callback);
                }
            }
            SetSubscriptionPollTimer();
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            lock (_subscribers)
            {
                _subscribers.Clear();
            }
        }
	}
}
