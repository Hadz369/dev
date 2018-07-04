﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace HS
{
    /* Trace Levels: 0=Off, 1=Error, 2=Warning, 3=Info, 4=Verbose */

    #region Listeners

    public class LogTracer : TextWriterTraceListener
    {
        TraceLevel _traceLevel;
        Object _locker = new Object();
        
        bool _isRolling = false;
        TracerFileOptions _fileOptions = null;

        public LogTracer(TracerFileOptions fileOptions)
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
                    TracerData td = data as TracerData;

                    if (td == null) td = new TracerData(data.ToString());

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
                TracerData td = o as TracerData;

                if (td == null) td = new TracerData(o.ToString());

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

    public class TracerFileOptions
    {
        private DateTime _dt;

        private string _path = "", _filePrefix = "", _baseFileName;
        private long _maxFileSize = 0;
        private bool _dailyRollover = false, _autoFlush = true;

        StreamWriter _streamWriter = null;

        public TracerFileOptions(string path, string filePrefix) : this(path, filePrefix, 0) { }

        public TracerFileOptions(string path, string filePrefix, int maxFileSize)
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

    public class TracerData
    {
        string _message = "";
        TraceLevel _level;
        bool _isPacketData = false, _forceOutput = false;
        Exception _exception = null;
        StackFrame _stackFrame = null;
        DateTime _eventDt = DateTime.Now;
        string _group = "Program";

        const byte STX = 0x02;
        const byte ETX = 0x03;
        const byte SEP = 0x1F;

        public TracerData(string message)
            : this(TraceLevel.Info, message)
        {
        }

        public TracerData(TraceLevel level, string message)
        {
            _level = level;
            _message = message;

            if (_level == TraceLevel.Error)
                _stackFrame = new StackFrame(2);
        }

        public TracerData(string message, Exception exception)
        {
            _message = message;
            _exception = exception;
            _level = TraceLevel.Error;
            _stackFrame = new StackFrame(2, true);
        }

        public TraceLevel TraceLevel { get { return _level; } }
        public string Message { get { return _message; } }

        public bool IsPacketData { get { return _isPacketData; } set { _isPacketData = value; } }
        [Obsolete("ForceOutput is obsolete. Use Always instead")]
        public bool ForceOutput { get { return _forceOutput; } set { _forceOutput = value; } }
        // This is a nicer name that ForceOutput
        public bool Always { get { return _forceOutput; } set { _forceOutput = value; } }

        public string Group { get { return _group; } set { _group = value; } }

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
            //string msg = String.Format("{1}{0}{2}{0}{3}{0}{4,-3}{0}{5,-12}{0}{6}", 
            string msg = String.Format("{1}{0}{2}{0}{3}{0}{4,-3}{0}{5}", 
                (char)160,
                _eventDt.ToString(Tracer.EventDateFormat),
                _eventDt.ToString(Tracer.EventTimeFormat),
                _level.ToString().ToUpper().Substring(0, 4),
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                //_group,
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

        public string ToLogXml()
        {
            //string msg = String.Format("{1}{0}{2}{0}{3}{0}{4,-3}{0}{5,-12}{0}{6}", 
            string msg = String.Format("<L><D>{0}</D><T>{1}</T><V>{2}</C><R>{3,-3}</R><M>{4}</M>",
                _eventDt.ToString(Tracer.EventDateFormat),
                _eventDt.ToString(Tracer.EventTimeFormat),
                _level.ToString().ToUpper().Substring(0, 4),
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                //_group,
                _message);

            if (_stackFrame != null)
                msg = String.Format("{0}<SF>Class={1}; Method={2}; Line={3};</SF>",
                    msg,
                    _stackFrame.GetMethod().DeclaringType.Name,
                    _stackFrame.GetMethod().Name,
                    _stackFrame.GetFileLineNumber());

            if (_exception != null)
            {
                msg = String.Concat(msg, "<EX>", _exception.Message);

                Exception ex = _exception.InnerException;
                while (ex != null)
                {
                    msg = String.Concat(msg, "<IX>", ex.Message, "</IX>");
                    ex = ex.InnerException;
                }
                msg = String.Concat(msg, "</EX>");
            }

            msg += "</L>";
            return msg;
        }

        public string GetXmlMessage()
        {
            return "";
        }
    }

    #endregion

    public static class Tracer
    {
        //static Stopwatch _sw;
        static UInt16 _process = 0;
        static string _logPath = "";
        static bool _multiUser = false;

        static SysInfo _sysInfo;

        static Tracer()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Remove("Default");

            _sysInfo = new SysInfo();

            //_sw = new Stopwatch();
        }

        public static void Initialise(string appKey, bool multiUser)
        {
            _multiUser = multiUser;
            string root = "";

            if (_multiUser) root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            else root = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            _logPath = Path.Combine(root, "HomeServer", "Log", appKey);
        }

        public static string LogFilePath { get { return _logPath; } }
        public static bool MultiUser { get { return _multiUser; } }

        public static string EventDateFormat { get { return "yyyy-MM-dd"; } }
        public static string EventTimeFormat { get { return "HH:mm:ss.fff"; } }
        public static string EventDateTimeFormat { get { return "yyyy-MM-ddTHH:mm:ss.fff"; } }

        public static SysInfo SysInfo { get { return _sysInfo; } }

        [Obsolete("Functionality has been disabled. Use Environment.Tick functions instead.")]
        public static long ElapsedMilliseconds { get { return 0; /*_sw.ElapsedMilliseconds;*/ } }

        [Obsolete("Functionality has been disabled. Use Environment.Tick functions instead.")]
        public static long ElapsedTicks { get { return 0; /*_sw.ElapsedMilliseconds;*/ } }

        [Obsolete("Functionality has been disabled. Use Environment.Tick functions instead.")]
        public static TimeSpan Elapsed { get { return new TimeSpan(); /*_sw.ElapsedMilliseconds;*/ } }

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
            //_sw.Start();
        }

        public static void Remove(string name)
        {
            Trace.Listeners.Remove(name);
            //if (Trace.Listeners.Count == 0)
            //    _sw.Stop();
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
            WriteLine(new TracerData(TraceLevel.Verbose, message) { Always = true });
        }

        public static void Debug(string message)
        {
            WriteLine(new TracerData(TraceLevel.Verbose, message));
        }

        public static void Info(string message)
        {
            WriteLine(new TracerData(TraceLevel.Info, message));
        }

        public static void Warning(string message)
        {
            WriteLine(new TracerData(TraceLevel.Warning, message));
       }

        public static void Error(string message)
        {
            WriteLine(new TracerData(TraceLevel.Error, message));
        }

        public static void Error(string message, Exception ex)
        {
            WriteLine(new TracerData(message, ex));
        }

        public static void Log(TracerData data)
        {
            WriteLine(data);
        }

        public static void Write(object message)
        {
            Trace.Write(message);
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
                LogTracer _logFileDailyListener = new LogTracer(new TracerFileOptions(_logPath, appKey)) { TraceLevel = traceLevel };
                Tracer.Add(_logFileDailyListener);
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
                Tracer.Add(consoleListener);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating console listener", ex);
            }
        }

        public static void CreateEventLogListener(string source)
        {
            try
            {
                EventLog log = new EventLog("HomeServer");
                log.Source = source;
                
                EventLogTraceListener listener = new EventLogTraceListener(log);

                Tracer.Add(listener);
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
    }
}