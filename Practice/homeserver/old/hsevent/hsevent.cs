using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeServer
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

    public class MessageEventData
    {
        EventLevel _lvl = EventLevel.Information;
        string _msg;

        public MessageEventData(string message)
        {
            _msg = message;
        }

        public MessageEventData(string message, EventLevel level)
        {
            _lvl = level;
            _msg = message;
        }

        public EventLevel EventLevel { get { return _lvl; } }
        public string Message { get { return _msg; } }
    }

    public class MetricEventData
    {
        Metric _metric;
        object _value;

        public MetricEventData(Metric metric, object value)
        {
            _metric = metric;
            _value = value;
        }

        public Metric Metric { get { return _metric; } }
        public object Value { get { return _value; } }
    }

    public enum EventLevel
    {
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }
}
