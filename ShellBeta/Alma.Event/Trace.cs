using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Alma.Event
{
    /* Trace Levels: 0=Off, 1=Error, 2=Warning, 3=Info, 4=Verbose */

    #region Listeners

    public class DjhTextWriterTraceListener : TextWriterTraceListener
    {
        const byte STX = 0x02;
        const byte ETX = 0x03;
        const byte SEP = 0x1F;

        TraceLevel _traceLevel;

        bool _isRolling = false;
        DjhTraceFileOptions _fileOptions = null;

        public DjhTextWriterTraceListener(DjhTraceFileOptions fileOptions)
        {
            _isRolling = true;
            _fileOptions = fileOptions;
            _traceLevel = System.Diagnostics.TraceLevel.Info;
        }

        public override string Name  { get { return base.Name; }    set { base.Name = value; } }
        public TraceLevel TraceLevel { get { return _traceLevel; }  set { _traceLevel = value; } }

        string FrameLogMessage2(DateTime dt, UInt16 process, TraceLevel level, TimeSpan ts, string heading, string msg)
        {
            return String.Format("{0}{3}{2} {4}{2} {5}{2} {6}{2} {7}{2} {8,-16}{2} {9}{1}",
                (char)STX, (char)ETX, (char)SEP,
                DateTime.Now.ToString(DjhTrace.EventTimeFormat),
                DjhTrace.ElapsedTicks,
                process.ToString("00000"),
                level.ToString().ToUpper().Substring(0, 4),
                (ts == null ? 0 : ts.TotalMilliseconds).ToString("00000.00"),
                heading.Length > 16 ? heading.Substring(0, 16) : heading,
                msg);
        }
        
        public override void Write(object o)
        {
            WriteMessage(o, false);
        }

        public override void WriteLine(object o)
        {
            WriteMessage(o, true);
        }

        private void WriteMessage(object o, bool newline)
        {
            string msg = "";
            Type t = o.GetType();
            bool force = false;
            TraceLevel level = System.Diagnostics.TraceLevel.Info;
            DateTime dt = DateTime.Now;

            // Format the output message
            if (t == typeof(DjhTraceData))
            {
                DjhTraceData x = o as DjhTraceData;
                force = x.ForceOutput;
                x.ForceOutput = false;  // Turn off the force flag in case the object is reused
                level = x.TraceLevel;
                msg =x.GetVerboseTextMessage();
            }
            else if (t == typeof(DjhMethodHelper))
            {
                DjhMethodHelper x = o as DjhMethodHelper;
                force = x.Always;
                x.Always = false; // Turn off the always flag in case the object is reused
                level = x.TraceLevel;
                msg = FrameLogMessage2(dt, x.ProcessId, level, x.Interval, x.Heading, x.FullMessage);
            }
            else
            {
                msg = FrameLogMessage2(dt, 0, level, new TimeSpan(), level.ToString(), o.ToString());
            }

            if (force || level <= _traceLevel)
            {
                // Maintain the output files
                if (_isRolling)
                    base.Writer = _fileOptions.ManageOutputStream();

                // Write the message
                if (newline) base.WriteLine(msg);
                else base.Write(msg);
            }
        }

        public override void Flush()
        {
            base.Flush();
        }
    }

    public class DjhConsoleWriterTraceListener : TextWriterTraceListener
    {
        TraceLevel _traceLevel = TraceLevel.Info;

        public DjhConsoleWriterTraceListener() : base(Console.Out) 
        {
            Console.BufferWidth = 160;
        }

        public override string Name { get { return base.Name; }  set { base.Name = value; } }

        public TraceLevel TraceLevel { get { return _traceLevel; }  set { _traceLevel = value; } }

        public override void Write(object o)
        {
            WriteMessage(o, false);
        }

        public override void WriteLine(object o)
        {
            WriteMessage(o, true);
        }

        private void WriteMessage(object o, bool newline)
        {
            string msg = "";
            Type t = o.GetType();
            bool force = false;
            TraceLevel level = System.Diagnostics.TraceLevel.Info;

            // Format the output message
            if (t == typeof(DjhTraceData))
            {
                DjhTraceData x = o as DjhTraceData;
                force = x.ForceOutput;
                x.ForceOutput = false;  // Turn off the force flag in case the object is reused
                level = x.TraceLevel;
                msg = x.GetBasicTextMessage();
            }
            else if (t == typeof(DjhMethodHelper))
            {
                DjhMethodHelper x = o as DjhMethodHelper;
                force = x.Always;
                x.Always = false; // Turn off the always flag in case the object is reused
                level = x.TraceLevel;
                msg = x.Message;
            }
            else
            {
                msg = o.ToString();
            }

            if (force || level <= _traceLevel)
            {
                SetColour(level);

                // Write the message
                if (newline) base.WriteLine(msg);
                else base.Write(msg);
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
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }

    #endregion

    #region Data Objects

    public class DjhTraceFileOptions
    {
        private DateTime _dt;

        private string _path = "", _filePrefix = "", _baseFileName;
        private long _maxFileSize = 0;
        private bool _dailyRollover = false, _autoFlush = true;

        StreamWriter _streamWriter = null;

        public DjhTraceFileOptions(string path, string filePrefix) : this(path, filePrefix, 0) { }

        public DjhTraceFileOptions(string path, string filePrefix, int maxFileSize)
        {
            _path = path;
            _filePrefix = filePrefix;
            this.MaxFileSize = maxFileSize;

            // Default to a daily rollover if the max file size is not set
            if (_maxFileSize == 0) _dailyRollover = true;

            SetBaseFileName();
        }

        public string FilePrefix   { get { return _filePrefix; }     set { _filePrefix = value; SetBaseFileName(); } }
        public string LogPath      { get { return _path; }           set { _path = value; SetBaseFileName(); } }
        public long   MaxFileSize  { get { return _maxFileSize; }    set { _maxFileSize = value*1024*1024; } }
        public bool   AutoFlush    { get { return _autoFlush; }      set { SetAutoFlush(value); } }
        public string BaseFileName { get { return _baseFileName; } }

        private void SetBaseFileName()
        {
            _baseFileName= Path.Combine(_path, _filePrefix);
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

    public class TraceData : IDisposable
    {
        string _message = "", _method = "", _sourceFilePath = "";
        TraceLevel _level;
        bool _isPacketData = false, _forceOutput = false, _methodComplete = false, _logCompleted = false;
        Exception _exception = null;
        long _sTicks = 0, _iTicks = 0; /* start ticks, interval ticks */
        long _sMillis = 0, _iMillis = 0; /* start ticks, interval ticks */
        UInt16 _process = 0;
        byte[] _bytes = null;
        DateTime _createDt, _eventDt;

        public TraceData(string message)
        {}

        public TraceLevel TraceLevel     { get { return _level; } }

        public string     Message        { get { return _message; } }
        public string     FullMessage    { get { return GetFullMessage(); } }
        
        public Exception  Exception      { get { return _exception; } }

        public string     MethodName     { get { return _method; } }
        public UInt16     ProcessId      { get { return _process; } }
        public long       ElapsedMillis  { get { return Tracer.ElapsedMilliseconds - _sMillis; } }

        public bool       IsPacketData   { get { return _isPacketData; } }
        public bool       Always         { get { return _forceOutput; }  set { _forceOutput = value; } }
        public bool       LogCompleted   { get { return _logCompleted; } set { _logCompleted = value; } }

        public override string ToString()
        {
            return FullMessage;
        }

        public bool MethodComplete { get { return _methodComplete; } set { _methodComplete = value; } }

        private string GetFullMessage()
        {
            string msg = _message;

            if (this.Exception != null)
            {
                msg = String.Concat(msg, Environment.NewLine, "Error Message:");

                Exception ex = this.Exception;

                msg = String.Concat(msg, Environment.NewLine, ex.Message);

                while (ex.InnerException != null)
                {
                    msg = String.Concat(msg, Environment.NewLine, ex.InnerException.Message);
                    ex = ex.InnerException;
                }
            }

            return msg;
        }

        private TimeSpan GetInterval(long startTicks)
        {
            TimeSpan ts;

            long ticks = Tracer.ElapsedTicks;

            return new TimeSpan(ticks - startTicks);
        }
    }

    [Obsolete]
    public class DjhMethodHelperX : IDisposable
    {
        string _message = "", _method = "", _header = "";
        TraceLevel _level;
        bool _isPacketData = false, _forceOutput = false, _methodComplete = false, _logCompleted = false;
        Exception _exception = null;
        long _sTicks = 0, _iTicks = 0; /* start ticks, interval ticks */
        long _sMillis = 0, _iMillis = 0; /* start ticks, interval ticks */
        UInt16 _process = 0;
        byte[] _bytes = null;
        DateTime _createDt, _eventDt;

        public DjhMethodHelperX()
        {
            _createDt = _eventDt = DateTime.Now;
            _sTicks = _iTicks = DjhTrace.ElapsedTicks;
            _sMillis = _iMillis = DjhTrace.ElapsedMilliseconds;
        }

        public DjhMethodHelperX(string methodName, UInt16 process)
            : this()
        {
            _method = methodName;
            _process = process;
        }

        public TraceMsg(string message)
        {
            SetMsg(TraceLevel.Info, message, null, null);
        }

        public TraceMsg(string heading, string message)
        {
            SetMsg(TraceLevel.Info, heading, message, null, null);
        }

        public SetMsg(TraceLevel level, string message)
        {
            TraceMsg(level, level.ToString(), message, null, null);
        }

        public SetMsg(TraceLevel level, string heading, string message)
        {
            TraceMsg(level, heading, message, null, null);
        }

        public TraceMsg(string message, Exception exception)
        {
            SetMsg(TraceLevel.Error, message, null, exception);
        }

        public TraceMsg(string heading, string message, Exception exception)
        {
            SetMsg(TraceLevel.Error, heading, message, null, exception);
        }

        public SetMsg(TraceLevel level, string message, byte[] bytes, Exception exception)
        {
            SetMsg(TraceLevel.Error, level.ToString(), message, null, exception);
        }

        public SetMsg(TraceLevel level, string heading, string message, byte[] bytes, Exception exception)
        {
            _level = level;
            _header = heading;
            _message = message;
            _exception = exception;
            _bytes = bytes;
            _eventDt = DateTime.Now;

            if (bytes != null) _isPacketData = true;
        }

        public TraceLevel TraceLevel { get { return _level; } }

        public string Heading { get { return _header; } }
        public string Message { get { return _message; } }
        public string FullMessage { get { return GetFullMessage(); } }

        public Exception Exception { get { return _exception; } }

        public string MethodName { get { return _method; } }
        public UInt16 ProcessId { get { return _process; } }
        public TimeSpan Elapsed { get { return GetElapsed(); } }
        public TimeSpan Interval { get { return GetInterval(); } }
        public long ElapsedMillis { get { return DjhTrace.ElapsedMilliseconds - _sMillis; } }

        public bool IsPacketData { get { return _isPacketData; } }
        public bool Always { get { return _forceOutput; } set { _forceOutput = value; } }
        public bool LogCompleted { get { return _logCompleted; } set { _logCompleted = value; } }

        public override string ToString()
        {
            return FullMessage;
        }

        public bool MethodComplete { get { return _methodComplete; } set { _methodComplete = value; } }

        private string GetFullMessage()
        {
            string msg = _message;

            if (this.Exception != null)
            {
                msg = String.Concat(msg, Environment.NewLine, "Error Message:");

                Exception ex = this.Exception;

                msg = String.Concat(msg, Environment.NewLine, ex.Message);

                while (ex.InnerException != null)
                {
                    msg = String.Concat(msg, Environment.NewLine, ex.InnerException.Message);
                    ex = ex.InnerException;
                }
            }

            return msg;
        }

        /// <summary>
        /// Get the timespan since the last call. This requires the process has been initialised.
        /// </summary>
        /// <returns></returns>
        private TimeSpan GetInterval()
        {
            TimeSpan ts;

            long ticks = DjhTrace.ElapsedTicks;

            // If the method is complete return the total method duration as the interval,
            // otherwise return the interval since the last 
            if (_methodComplete) ts = new TimeSpan(ticks - _sTicks);
            else ts = new TimeSpan(ticks - _iTicks);

            _iTicks = ticks;

            return ts;
        }

        /// <summary>
        /// Get the elapsed timespan since the process started.
        /// </summary>
        /// <returns></returns>
        private TimeSpan GetElapsed()
        {
            return new TimeSpan(DjhTrace.ElapsedTicks - _sTicks);
        }

        public void Dispose()
        {
            if (_logCompleted)
            {
             //   DjhTrace.MethodEnd(this);
            }
        }
    }

    [Obsolete("DjhTraceData will not be supported in a future version. Replace with DjhMethodHelper")]
    public class DjhTraceData
    {
        string _message = "";
        TraceLevel _level;
        bool _isPacketData = false, _forceOutput = false;
        Exception _exception = null;
        StackFrame _stackFrame = null;
        DateTime _eventDt = DateTime.Now;

        public DjhTraceData(string message)
            : this(TraceLevel.Info, message)
        {
        }

        public DjhTraceData(TraceLevel level, string message)
        {
            _level = level;
            _message = message;

            if (_level == TraceLevel.Error)
                _stackFrame = new StackFrame(2);
        }

        public DjhTraceData(string message, Exception exception)
        {
            _message = message;
            _exception = exception;
            _level = TraceLevel.Error;
            _stackFrame = new StackFrame(2, true);
        }

        public TraceLevel TraceLevel { get { return _level; } }
        public string Message { get { return _message; } }

        public bool IsPacketData { get { return _isPacketData; } set { _isPacketData = value; } }
        public bool ForceOutput { get { return _forceOutput; } set { _forceOutput = value; } }

        public StackFrame StackFrame { get { return _stackFrame; } }
        public Exception Exception { get { return _exception; } }

        public override string ToString()
        {
            return _message;
        }

        public string GetBasicTextMessage()
        {
            return String.Format("{0} {1} {2}", _eventDt.ToString(DjhTrace.EventTimeFormat), _level.ToString().ToUpper().Substring(0, 4), _message);
        }

        public string GetVerboseTextMessage()
        {
            string msg = String.Format("{0} {1} {2}", _eventDt.ToString(DjhTrace.EventTimeFormat), _level.ToString().ToUpper().Substring(0, 4), _message);

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

        public string GetXmlMessage()
        {
            return "";
        }
    }

    #endregion

    public static class Tracer
    {
        static Stopwatch _sw;
        static UInt16 _process = 0;

        static Tracer()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Remove("Default");

            _sw = new Stopwatch();
        }

        public static void Initialise()
        {
        }

        public static string EventTimeFormat { get { return "yyyy-MM-ddTHH:mm:ss.fff"; } }

        public static long ElapsedMilliseconds { get { return _sw.ElapsedMilliseconds; } }        
        public static long ElapsedTicks { get { return _sw.ElapsedTicks; } }

        public static TimeSpan Elapsed { get { return _sw.Elapsed; } }

        public static TimeSpan GetTimeSpan(long ticks)
        {
            return new TimeSpan(ticks);
        }

        public static void Add(TraceListener listener)
        {
            Trace.Listeners.Add(listener);
            _sw.Start();
        }

        public static void Remove(string name)
        {
            Trace.Listeners.Remove(name);
            if (Trace.Listeners.Count == 0)
                _sw.Stop();
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

        /// <summary>
        /// Use this one to write a simple message that is disposed properly
        /// </summary>
        /// <returns></returns>
        public static DjhMethodHelper SimpleMsg()
        {
            return new DjhMethodHelper() { LogCompleted = false };
        }

        public static DjhMethodHelper MethodStart(string methodName)
        {
            return MethodStart(methodName, "");
        }

        public static DjhMethodHelper MethodStart(string methodName, string message)
        {
            DjhMethodHelper mh = new DjhMethodHelper(methodName, GetProcessId()) { Always = true };

            string msg = methodName;
            
            if (message != "")
                msg = String.Concat(msg, ", ", message);

            mh.TraceMsg("Method Start", msg);

            WriteLine(mh);

            return mh;
        }

        public static void MethodEnd(DjhMethodHelper mh)
        {
            mh.TraceMsg("Method End", mh.MethodName);
            mh.Always = true;
            mh.MethodComplete = true;            
            WriteLine(mh);
        }

        [Obsolete("DjhTraceData will not be supported in a future version. Replace with DjhMethodHelper")]
        public static void Write(DjhTraceData traceData)
        {
            Trace.Write(traceData);
        }

        [Obsolete("DjhTraceData will not be supported in a future version. Replace with DjhMethodHelper")]
        public static void WriteLine(DjhTraceData traceData)
        {
            Trace.WriteLine(traceData);
        }

        public static void Write(object message)
        {
            Trace.Write(message);
        }
        
        public static void WriteLine(object message)
        {
            Trace.WriteLine(message);
        }

        public static void WriteLine(TraceLevel level, string message)
        {
            DjhMethodHelper mh = new DjhMethodHelper();
            mh.TraceMsg(level, message);
            Trace.WriteLine(mh);
        }

    }
}
