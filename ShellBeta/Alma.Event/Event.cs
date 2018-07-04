using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Alma.Event
{
    public delegate void TaskEventDelegate(object eventData);
    public delegate void MessageEventDelegate(MessageEventData eventData);
    public delegate void DbConnectionEventDelegate(DbConnectionEventData eventData);
    public delegate void SqlExecutionEventDelegate(SqlExecutionEventData eventData);

    public class FlexEventHandler
    {
        #region Singleton Initialisation

        private static readonly FlexEventHandler instance = new FlexEventHandler();

        private FlexEventHandler()
        {
        }

        public static FlexEventHandler Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        //public event TaskEventDelegate taskEvent;
        public event MessageEventDelegate MessageEvent;
        public event DbConnectionEventDelegate DbConnectionEvent;
        public event SqlExecutionEventDelegate SqlExecutionEvent;

        ExecutionMode _mode = ExecutionMode.Console;

        public ExecutionMode ExecutionMode { get { return _mode; } set { _mode = value; } }

        public void RaiseEvent(object eventData)
        {
            if (eventData.GetType() == typeof(MessageEventData) && MessageEvent != null)
                MessageEvent((MessageEventData)eventData);
            else if (eventData.GetType() == typeof(DbConnectionEventData) && DbConnectionEvent != null)
                DbConnectionEvent((DbConnectionEventData)eventData);
            else if (eventData.GetType() == typeof(SqlExecutionEventData) && SqlExecutionEvent != null)
                SqlExecutionEvent((SqlExecutionEventData)eventData);

        }

        public void MessageEventHandler(MessageEventData e)
        {
            if (_mode == ExecutionMode.Console)
                Console.WriteLine(e.message);
        }
    }

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

    public class DbConnectionEventData : BaseEventData
    {
        public DbConnectionEventData(string Message)
        {
            _msg = Message;
        }
    }

    public class SqlExecutionEventData : BaseEventData
    {
        public SqlExecutionEventData(string statement, string parms, DateTime startdt, int duration)
        {
            _msg = String.Format("Sql Execution: Stmt={0}, Parms={1}, StartDT={2}, Duration={3}", statement, parms, startdt.ToString(), duration);
        }
    }

    public class MessageEventData : BaseEventData
    {
        MessageEventData _source;
        EventLevel _level;

        /// <summary>
        /// A general informational message
        /// </summary>
        /// <param name="eventLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(string message)
        {
            _msg = message;
            _level = EventLevel.Information;
        }

        /// <summary>
        /// A general message with specified level
        /// </summary>
        /// <param name="eventLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(EventLevel eventLevel, string message)
        {
            _msg = message;
            _level = eventLevel;
        }

        /// <summary>
        /// A general message with specified level and exception detail
        /// </summary>
        /// <param name="eventLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(EventLevel eventLevel, string message, Exception exception)
        {
            _msg = message;
            _level = eventLevel;
            _ex = exception;
        }

        /// <summary>
        /// A general informational message associated with another process identified by the process guid
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="eventLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(Guid guid, string message)
        {
            _guid = guid;
            _msg = message;
            _level = EventLevel.Information;
        }

        /// <summary>
        /// A general message associated with another process identified by the process guid
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="eventLevel"></param>
        /// <param name="message"></param>
        public MessageEventData(Guid guid, EventLevel eventLevel, string message)
        {
            _guid = guid;
            _msg = message;
            _level = eventLevel;
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

            if (isFatalError) _level = EventLevel.Fatal;
            else _level = EventLevel.Error;
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
        public EventLevel level { get { return _level; } }
        public MessageEventData Source { get { return _source; } set { _source = value; } }
    }

    public class DatabaseOperationEventData : BaseEventData
    {
        string _db;
        DatabaseBackupType _type;
        DatabaseOperationMode _mode;
        JobState _state;

        /// <summary>
        /// Used to signal the start of an operation
        /// </summary>
        /// <param name="database"></param>
        /// <param name="type"></param>
        public DatabaseOperationEventData(string database, DatabaseBackupType type, DatabaseOperationMode mode)
        {
            _state = JobState.Starting;
            _db = database;
            _type = type;
            _guid = Guid.NewGuid();
            _mode = mode;
        }

        /// <summary>
        /// Used to signal the successful completion an operation
        /// </summary>
        /// <param name="database"></param>
        /// <param name="type"></param>
        public DatabaseOperationEventData(Guid guid)
        {
            _state = JobState.Completed;
            _guid = guid;
        }

        /// <summary>
        /// Used to signal the failure of an operation
        /// </summary>
        /// <param name="database"></param>
        /// <param name="type"></param>
        public DatabaseOperationEventData(Guid guid, Exception exception)
        {
            _guid = guid;
            _ex = exception;
            _state = JobState.Failed;
        }

        public JobState state { get { return _state; } }
        public string database { get { return _db; } }
        public DatabaseBackupType type { get { return _type; } }
        public DatabaseOperationMode mode { get { return _mode; } }
    }

    public class FileCopyEventData : BaseEventData
    {
        JobState _state;
        string _file;

        /// <summary>
        /// Used to signal the start of a file copy. Creates a new guid.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="type"></param>
        public FileCopyEventData(string file)
        {
            _state = JobState.Starting;
            _file = file;
            _guid = Guid.NewGuid();
        }

        public FileCopyEventData(Guid guid)
        {
            _state = JobState.Completed;
        }

        public FileCopyEventData(Guid guid, Exception exception)
        {
            _state = JobState.Failed;
            _guid = guid;
        }

        public JobState state { get { return _state; } }
        public string file { get { return _file; } }
    }
}
