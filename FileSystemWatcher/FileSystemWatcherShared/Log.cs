using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace FSW
{
    /* Trace Levels: 0=Off, 1=Error, 2=Warning, 3=Info, 4=Verbose */

    #region Listeners

    public class LogTracer : TextWriterTraceListener
    {
        TraceLevel _traceLevel;
        Object _locker = new Object();
        
        bool _isRolling = false;
        LogFileOptions _fileOptions = null;

        public LogTracer(LogFileOptions fileOptions)
        {
            _isRolling = true;
            _fileOptions = fileOptions;
            _traceLevel = System.Diagnostics.TraceLevel.Info;
        }

        public override string Name { get { return base.Name; } set { base.Name = value; } }
        public TraceLevel TraceLevel { get { return _traceLevel; } set { _traceLevel = value; } }

        public override void Write(object data)
        {
            WriteMessage(data, false);
        }

        public override void WriteLine(object data)
        {
            WriteMessage(data, true);
        }

        private void WriteMessage(object data, bool newline)
        {
            try
            {
                lock (_locker)
                {
                    LogData td = data as LogData;

                    if (td == null) td = new LogData(data.ToString());

                    if (td.Always || td.TraceLevel <= _traceLevel)
                    {
                        // Maintain the output files
                        if (_isRolling)
                            base.Writer = _fileOptions.ManageOutputStream();

                        // Write the message
                        if (newline) base.WriteLine(td.ToLogString());
                        else base.Write(td.ToString());
                    }
                }
            }
            catch (IOException)
            {
                // Not sure what to do here if this fails
            }
        }

        public override void Flush()
        {
            base.Flush();
        }
    }

    public class ConsoleTracer : TextWriterTraceListener
    {
        TraceLevel _traceLevel = TraceLevel.Info;
        Object _locker = new Object();

        public ConsoleTracer()
            : base(Console.Out)
        {
            Console.BufferWidth = 160;
        }

        public override string Name { get { return base.Name; } set { base.Name = value; } }

        public TraceLevel TraceLevel { get { return _traceLevel; } set { _traceLevel = value; } }

        public override void Write(object message)
        {
            WriteMessage(message, false);
        }

        public override void WriteLine(object message)
        {
            WriteMessage(message, true);
        }
        
        private void WriteMessage(object o, bool newline)
        {
            lock (_locker)
            {
                // Format the output message
                LogData td = o as LogData;

                if (td == null) td = new LogData(o.ToString());

                if (td.Always || td.TraceLevel <= _traceLevel)
                {
                    SetColour(td.TraceLevel);

                    // Write the message
                    if (newline) base.WriteLine(td.ToConsoleString());
                    else base.Write(td.ToConsoleString());
                }
            }
        }

        private void SetColour(TraceLevel level)
        {
            switch (level)
            {
                case System.Diagnostics.TraceLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case System.Diagnostics.TraceLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case System.Diagnostics.TraceLevel.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }

    #endregion

    #region Data Objects

    public class LogFileOptions
    {
        private DateTime _dt;

        private string _path = "", _filePrefix = "", _baseFileName;
        private long _maxFileSize = 0;
        private bool _dailyRollover = false, _autoFlush = true;

        StreamWriter _streamWriter = null;

        public LogFileOptions(string path, string filePrefix) : this(path, filePrefix, 0) { }

        public LogFileOptions(string path, string filePrefix, int maxFileSize)
        {
            _path = path;
            _filePrefix = filePrefix;
            this.MaxFileSize = maxFileSize;

            // Default to a daily rollover if the max file size is not set
            if (_maxFileSize == 0) _dailyRollover = true;

            SetBaseFileName();
        }

        public string FilePrefix { get { return _filePrefix; } set { _filePrefix = value; SetBaseFileName(); } }
        public string LogPath { get { return _path; } set { _path = value; SetBaseFileName(); } }
        public long MaxFileSize { get { return _maxFileSize; } set { _maxFileSize = value * 1024 * 1024; } }
        public bool AutoFlush { get { return _autoFlush; } set { SetAutoFlush(value); } }
        public string BaseFileName { get { return _baseFileName; } }

        private void SetBaseFileName()
        {
            _baseFileName = Path.Combine(_path, _filePrefix);
        }

        private void SetAutoFlush(bool value)
        {
            _autoFlush = value;
            if (_streamWriter != null)
                _streamWriter.AutoFlush = _autoFlush;
        }

        public StreamWriter ManageOutputStream()
        {
            bool changed = false;
            string logFile = "";
            DateTime now = DateTime.Now;

            if (_dailyRollover)
            {
                // Manage log files over a rolling 7 day cycle.
                if (_streamWriter == null || _dt.DayOfWeek != now.DayOfWeek)
                {
                    changed = true;
                    _dt = now;

                    // Append the day name and extension to the base file name.
                    logFile = String.Format("{0}.{1}.log", _baseFileName, now.DayOfWeek.ToString());

                    // If the new file exists but it was not created today then delete it, otherwise append to it.
                    if (File.Exists(logFile))
                        if (File.GetLastWriteTime(logFile).Date != now.Date)
                            File.Delete(logFile);
                }
            }
            else
            {
                if (_streamWriter == null || _streamWriter.BaseStream.Length >= _maxFileSize)
                {
                    DateTime firstDt;
                    long tFrom, tTo, tDiff;

                    changed = true;

                    // Get the number of ticks since 01/01/2014 (an arbitrary date picked to make the number a little smaller)
                    firstDt = new DateTime(2014, 01, 01);
                    tFrom = firstDt.Ticks;
                    tTo = now.Ticks;

                    // Get the elapsed seconds since 01/01/2014
                    tDiff = (long)(new TimeSpan(tTo - tFrom)).TotalSeconds;

                    // Append the seconds to the base file name
                    logFile = String.Format("{0}.{1}.log", _baseFileName, tDiff.ToString());
                }
            }

            if (changed)
            {
                // Close the current file
                if (_streamWriter != null) _streamWriter.Close();

                // Create a new stream writer
                _streamWriter = new StreamWriter(new FileStream(logFile, FileMode.Append)) { AutoFlush = _autoFlush };
            }

            return _streamWriter;
        }
    }

    public class LogData
    {
        string _message = "";
        TraceLevel _level = TraceLevel.Off;
        bool _isPacketData = false, _forceOutput = false;
        Exception _exception = null;
        StackFrame _stackFrame = null;
        DateTime _eventDt = DateTime.Now;
        string _group = "Program";

        const byte STX = 0x02;
        const byte ETX = 0x03;
        const byte SEP = 0x1F;

        public LogData(string message)
            : this(TraceLevel.Info, message)
        {
        }

        public LogData(TraceLevel level, string message) : this(level, "Program", message, null) { }
        
        public LogData(TraceLevel level, string group, string message) : this(level, group, message, null) { }

        public LogData(TraceLevel level, string message, Exception exception) : this(level, "Program", message, exception) {}

        public LogData(TraceLevel level, string group, string message, Exception exception)
        {
            _level = level;
            _group = group;
            _message = message;
            _exception = exception;
        
            if (_level == TraceLevel.Error)
                _stackFrame = new StackFrame(2);
        }

        public TraceLevel TraceLevel { get { return _level; } }
        public string Message { get { return _message; } }
        public string Group { get { return _group; } set { _group = value; } }

        public bool IsPacketData { get { return _isPacketData; } set { _isPacketData = value; } }

        public bool Always { get { return _forceOutput; } set { _forceOutput = value; } }

        public StackFrame StackFrame { get { return _stackFrame; } }
        public Exception Exception { get { return _exception; } }

        public override string ToString()
        {
            return _message;
        }

        public string ToConsoleString()
        {
            return ToLogString();
        }

        public string ToLogString()
        {
            string msg = String.Format("{1}{0}{2}{0}{3,-4}{0}{4,-2}{0}{5,-8}{0}{6}", 
            //string msg = String.Format("{1}{0}{2}{0}{3}{0}{4,-3}{0}{5}", 
                (char)160,
                _eventDt.ToString(Log.EventDateFormat),
                _eventDt.ToString(Log.EventTimeFormat),
                _level == System.Diagnostics.TraceLevel.Off ? "OFF" : _level.ToString().ToUpper().Substring(0, 4),
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                (_group.Length <= 8 ? _group : _group.Substring(0,8)).ToUpper(),
                _message);

            if (_stackFrame != null)
                msg = String.Format("{0}; Class={1}; Method={2}; Line={3};",
                    msg,
                    _stackFrame.GetMethod().DeclaringType.Name,
                    _stackFrame.GetMethod().Name,
                    _stackFrame.GetFileLineNumber());

            if (_exception != null)
            {
                msg = String.Concat(msg, "ExMessage=", _exception.Message);

                Exception ex = _exception.InnerException;
                while (ex != null)
                {
                    msg = String.Concat(msg, "InnerExMessage=", ex.Message, ";");
                    ex = ex.InnerException;
                }
            }
            return msg;
        }
    }

    #endregion

    public static class Log
    {
        static UInt16 _process = 0;
        static string _logPath = "", _appKey = "MyApplication";

        static Log()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Remove("Default");
        }

        public static void Initialise(string appKey, bool isServiceLogger)
        {
            string root = "";

            IsServiceLogger = isServiceLogger;

            if (IsServiceLogger) 
                root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            else 
                root = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            _logPath = Path.Combine(root, "HadzNet", "Log", appKey);
        }

        public static bool   IsServiceLogger { get; private set; }
        public static string LogFilePath     { get { return _logPath; } }

        public static string EventDateFormat     { get { return "yyyy-MM-dd"; } }
        public static string EventTimeFormat     { get { return "HH:mm:ss.fff"; } }
        public static string EventDateTimeFormat { get { return "yyyy-MM-ddTHH:mm:ss.fff"; } }

        public static TraceListenerCollection Listeners { get { return Trace.Listeners; } }

        public static TraceListener GetListener(Type type)
        {
            foreach (var listener in Trace.Listeners)
            {
                if (listener.GetType() == type)
                    return listener as TraceListener;
            }

            return null;
        }

        public static TimeSpan GetTimeSpan(long ticks)
        {
            return new TimeSpan(ticks);
        }

        public static void Add(TraceListener listener)
        {
            Trace.Listeners.Add(listener);
        }

        public static void Remove(string name)
        {
            Trace.Listeners.Remove(name);
        }

        /// <summary>
        /// This is purely to keep all processes the same length for the log file.
        /// </summary>
        /// <returns></returns>
        private static UInt16 GetProcessId()
        {
            _process++;

            if (_process < 10000) _process = 10000;

            return _process;
        }

        public static void Always(string message)
        {
            WriteLine(new LogData(TraceLevel.Verbose, message) { Always = true });
        }

        public static void Always(string group, string message)
        {
            WriteLine(new LogData(TraceLevel.Verbose, group, message) { Always = true });
        }

        public static void Debug(string message)
        {
             WriteLine(new LogData(TraceLevel.Verbose, message));
        }

        public static void Debug(string group, string message)
        {
            WriteLine(new LogData(TraceLevel.Verbose, group, message));
        }

        public static void Info(string message)
        {
            WriteLine(new LogData(TraceLevel.Info, message));
        }

        public static void Info(string group, string message)
        {
            WriteLine(new LogData(TraceLevel.Info, group, message));
        }

        public static void Warning(string message)
        {
            WriteLine(new LogData(TraceLevel.Warning, message));
        }

        public static void Warning(string group, string message)
        {
            WriteLine(new LogData(TraceLevel.Warning, group, message));
        }

        public static void Error(string message)
        {
            WriteLine(new LogData(TraceLevel.Error, message));
        }

        public static void Error(string message, Exception ex)
        {
            WriteLine(new LogData(TraceLevel.Error,  message, ex));
        }

        public static void Error(string group, string message, Exception ex)
        {
            WriteLine(new LogData(TraceLevel.Error, group, message, ex));
        }

        public static void Fatal(string message, Exception ex)
        {
            WriteLine(new LogData(TraceLevel.Error, message, ex));
        }

        public static void Fatal(string group, string message, Exception ex)
        {
            WriteLine(new LogData(TraceLevel.Error, group, message, ex));
        }

        public static void WriteLine(object message)
        {
            Trace.WriteLine(message);
        }

        public static void CreateLogFileListener(string appKey, TraceLevel traceLevel)
        {
            try
            {
                if (!Directory.Exists(_logPath)) Directory.CreateDirectory(_logPath);

                // Create a daily file trace listener.
                LogTracer _logFileDailyListener = new LogTracer(new LogFileOptions(_logPath, appKey)) { TraceLevel = traceLevel };
                Log.Add(_logFileDailyListener);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating log file listener", ex);
            }
        }

        public static void CreateConsoleListener(TraceLevel traceLevel)
        {
            try
            {
                ConsoleTracer consoleListener = new ConsoleTracer() { TraceLevel = traceLevel };
                Log.Add(consoleListener);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating console listener", ex);
            }
        }

        public static void CreateEventLogListener(string source, TraceLevel level)
        {
            try
            {
                SourceLevels slevel;

                switch (level)
                {
                    case TraceLevel.Info:
                        slevel = SourceLevels.Information;
                        break;
                    case TraceLevel.Warning:
                        slevel = SourceLevels.Warning;
                        break;
                    case TraceLevel.Error:
                        slevel = SourceLevels.Error;
                        break;
                    case TraceLevel.Verbose:
                        slevel = SourceLevels.Verbose;
                        break;
                    default:
                        slevel = SourceLevels.Off;
                        break;
                }

                if (slevel != SourceLevels.Off)
                {
                    EventLog log = new EventLog("HadzNet");
                    log.Source = source;
                    EventLogTraceListener listener = new EventLogTraceListener(log);
                    listener.Filter = new EventTypeFilter(slevel);
                    Log.Add(listener);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating event log listener", ex);
            }
        }

        public static System.Diagnostics.TraceLevel GetTraceLevel(string level)
        {
            switch (level.ToLower())
            {
                case "info":
                    return System.Diagnostics.TraceLevel.Info;
                case "error":
                    return System.Diagnostics.TraceLevel.Error;
                case "warning":
                    return System.Diagnostics.TraceLevel.Warning;
                case "debug":
                    return System.Diagnostics.TraceLevel.Verbose;
                default:
                    return System.Diagnostics.TraceLevel.Off;
            }
        }

        public static void ConfigureDefaultTrace(string assemblyName)
        {
            Log.Initialise(assemblyName, false);

            System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;

            if (!IsServiceLogger && appSettings.AllKeys.Contains("ConsoleTraceLevel"))
                Log.CreateConsoleListener(Log.GetTraceLevel(appSettings["ConsoleTraceLevel"]));

            if (appSettings.AllKeys.Contains("LogTraceLevel"))
                Log.CreateLogFileListener(assemblyName, Log.GetTraceLevel(appSettings["LogTraceLevel"]));

            if (appSettings.AllKeys.Contains("EventLogTraceLevel"))
                Log.CreateEventLogListener(_appKey, Log.GetTraceLevel(appSettings["EventLogTraceLevel"]));

            Log.Info(String.Concat("Starting ", assemblyName, ": LogPath = ", Log.LogFilePath));
        }

    }
}