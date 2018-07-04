using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace HS
{
    public class BaseEventData : EventArgs
    {
        protected string _msg, _owner;
        protected Guid _guid;
        protected DateTime _now = DateTime.Now;
        protected Exception _ex;

        public string Message { get { return _msg; } }
        public string Owner { get { return _owner; } }
        public Guid OwnerGuid { get { return _guid; } }
        public DateTime EventDT { get { return _now; } }
        public Exception Exception { get { return _ex; } }
    }

    public class MessageEventData : BaseEventData
    {
        TraceLevel _level;
        ConsoleColor _colour;
        bool _hasCustomColour = false;

        /// <summary>
        /// A general informational message
        /// </summary>
        /// <param name="TraceLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(string message)
        {
            _msg = message;
            _level = TraceLevel.Info;
        }

        /// <summary>
        /// A general informational message with a specified colour
        /// </summary>
        /// <param name="TraceLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(string message, ConsoleColor colour)
        {
            _msg = message;
            _level = TraceLevel.Info;
            _colour = colour;
            _hasCustomColour = true;
        }

        /// <summary>
        /// A general message with specified level
        /// </summary>
        /// <param name="TraceLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(TraceLevel TraceLevel, string message)
        {
            _msg = message;
            _level = TraceLevel;
        }

        /// <summary>
        /// A general message with specified level and exception detail
        /// </summary>
        /// <param name="TraceLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(TraceLevel TraceLevel, string message, Exception exception)
        {
            _msg = message;
            _level = TraceLevel;
            _ex = exception;
        }

        /// <summary>
        /// A general informational message associated with another process identified by the process guid
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="TraceLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(Guid guid, string message)
        {
            _guid = guid;
            _msg = message;
            _level = TraceLevel.Info;
        }

        /// <summary>
        /// A general message associated with another process identified by the process guid
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="TraceLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(Guid guid, TraceLevel TraceLevel, string message)
        {
            _guid = guid;
            _msg = message;
            _level = TraceLevel;
        }

        /// <summary>
        /// An error message not associated with another process. If the message is fatal the whole process should stop.
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="guid"></param>
        /// <param name="exception"></param>
        public MessageEventData(Guid guid, Exception exception, bool isFatalError)
        {
            _guid = guid;
            _ex = exception;

            if (isFatalError) _level = TraceLevel.Error;
            else _level = TraceLevel.Error;
        }

        /// <summary>
        /// A non fatal error message associated with another process identified by the process guid 
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="guid"></param>
        /// <param name="exception"></param>
        public MessageEventData(Guid guid, Exception exception)
        {
            _guid = guid;
            _ex = exception;
        }

        public string message { get { return _msg; } }
        public TraceLevel level { get { return _level; } }
        public bool HasCustomColour { get { return _hasCustomColour; } }
        public ConsoleColor colour { get { return _colour; } }
    }

    public class MetricEventData : BaseEventData
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


}
