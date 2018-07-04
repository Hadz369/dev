using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS
{
    public delegate void MessageEventDelegate(MessageEventData EventData);
    public delegate void MetricEventDelegate(MetricEventData EventData);

    public sealed class dhEventHandler
    {
        #region Singleton Initialisation

        private static readonly dhEventHandler instance = new dhEventHandler();
        private dhEventHandler() { }
        public static dhEventHandler Instance { get { return instance; } }
        
        #endregion

        ExecutionMode _mode = ExecutionMode.Console;

        public event MessageEventDelegate MessageEvent;

        public ExecutionMode ExecutionMode { get { return _mode; } set { _mode = value; } }

        public void RaiseEvent(string message)
        {
            RaiseEvent(new MessageEventData(message));
        }

        public void RaiseEvent(object eventData)
        {
            if (eventData.GetType() == typeof(MessageEventData) && MessageEvent != null)
                MessageEvent((MessageEventData)eventData);
        }
    }
}
