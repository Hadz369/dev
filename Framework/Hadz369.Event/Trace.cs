using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Hadz369.Framework
{
    /* Trace Levels: 0=Off, 1=Error, 2=Warning, 3=Info, 4=Verbose */

    #region Listeners

    public class H369TextWriterTraceListener : TextWriterTraceListener
    {
        TraceLevel _defaultTraceLevel = TraceLevel.Error;
        TraceLevel _traceLevel;

        bool _isRolling = false;
        H369TraceFileOptions _fileOptions = null;

        public H369TextWriterTraceListener(H369TraceFileOptions fileOptions)
        {
            _isRolling = true;
            _fileOptions = fileOptions;
            _traceLevel = _defaultTraceLevel;
        }

        public override string Name  { get { return base.Name; }    set { base.Name = value; } }
        public TraceLevel TraceLevel { get { return _traceLevel; }  set { _traceLevel = value; } }

        public override void Write(object o)
        {
            if (o.GetType() == typeof(H369TraceData))
            {
                H369TraceData data = (H369TraceData)o;

                if (data.ForceOutput || data.TraceLevel <= _traceLevel)
                {
                    if (_isRolling)
                        base.Writer = _fileOptions.ManageOutputStream();

                    base.Write(FormatOutput(data));
                }
            }
            else base.Write(o);
        }

        public override void WriteLine(object o)
        {
            if (o.GetType() == typeof(H369TraceData))
            {
                H369TraceData data = (H369TraceData)o;

                if (data.ForceOutput || data.TraceLevel <= _traceLevel)
                {
                    if (_isRolling)
                        base.Writer = _fileOptions.ManageOutputStream();

                    base.WriteLine(FormatOutput(data));
                }
            }
            else base.WriteLine(o);
        }

        public override void Flush()
        {
            base.Flush();
        }

        string FormatOutput(H369TraceData data)
        {
            return data.GetVerboseTextMessage();
        }
    }

    public class H369ConsoleWriterTraceListener : TextWriterTraceListener
    {
        TraceLevel _traceLevel = TraceLevel.Info;

        public H369ConsoleWriterTraceListener() : base(Console.Out) 
        {
            Console.BufferWidth = 160;
        }

        public override string Name { get { return base.Name; }  set { base.Name = value; } }

        public TraceLevel TraceLevel { get { return _traceLevel; }  set { _traceLevel = value; } }

        public override void Write(object o)
        {
            if (o.GetType() == typeof(H369TraceData))
            {
                H369TraceData data = (H369TraceData)o;

                if (data.ForceOutput || data.TraceLevel <= _traceLevel)
                {
                    SetColour(data.TraceLevel);
                    base.Write(FormatOutput(data));
                }
            }
            else base.Write(o);
        }

        public override void WriteLine(object o)
        {
            if (o.GetType() == typeof(H369TraceData))
            {
                H369TraceData data = (H369TraceData)o;

                if (data.ForceOutput || data.TraceLevel <= _traceLevel)
                {
                    SetColour(data.TraceLevel);
                    base.WriteLine(data.GetBasicTextMessage());
                }
            }
            else base.WriteLine(o);
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

        string FormatOutput(H369TraceData data)
        {
            return data.GetBasicTextMessage();
        }
    }

    public class H369MessageEventListener : TraceListener
    {
        public event MessageEventDelegate MessageEvent;

        TraceLevel _traceLevel = TraceLevel.Info;

        public H369MessageEventListener() { }

        public override string Name { get { return base.Name; } set { base.Name = value; } }

        public TraceLevel TraceLevel { get { return _traceLevel; } set { _traceLevel = value; } }

        public override void Write(object o) 
        {            
        }

        public override void Write(string message) 
        {            
        }

        public override void WriteLine(string message)
        {
        }

        public override void WriteLine(object o)
        {
            if (o.GetType() == typeof(H369TraceData))
            {
                H369TraceData data = (H369TraceData)o;

                if (data.ForceOutput || data.TraceLevel <= _traceLevel)
                {
                    if (MessageEvent != null)
                        MessageEvent(new MessageEventData(data.Message));
                }
            }
            else base.WriteLine(o);
        }

        string FormatOutput(H369TraceData data)
        {
            return data.GetBasicTextMessage();
        }
    }

    #endregion

    #region Data Objects

    public class H369TraceFileOptions
    {
        private DateTime _dt;

        private string _path = "", _filePrefix = "", _baseFileName;
        private long _maxFileSize = 0;
        private bool _dailyRollover = false, _autoFlush = true;

        StreamWriter _streamWriter = null;

        public H369TraceFileOptions(string path, string filePrefix) : this(path, filePrefix, 0) { }

        public H369TraceFileOptions(string path, string filePrefix, int maxFileSize)
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

    public class H369TraceData
    {
        string _message = "";
        TraceLevel _level;
        bool _isPacketData = false, _forceOutput = false;
        Exception _exception = null;
        StackFrame _stackFrame = null;
        DateTime _eventDt = DateTime.Now;

        public H369TraceData(string message)
            : this(TraceLevel.Info, message)
        {
        }

        public H369TraceData(TraceLevel level, string message)
        {
            _level = level;
            _message = message;
        }

        public H369TraceData(string message, Exception exception)
        {
            _message = message;
            _exception = exception;
            _level = TraceLevel.Error;
            _stackFrame = new StackFrame(2, true);
        }

        public TraceLevel TraceLevel { get { return _level; } }
        public string Message { get { return _message; } }

        public bool IsPacketData { get { return _isPacketData; } set { _isPacketData = value; } }
        public bool ForceOutput  { get { return _forceOutput; }  set { _forceOutput = value; } }

        public StackFrame StackFrame { get { return _stackFrame; } }
        public Exception Exception { get { return _exception; } }

        public override string ToString()
        {
            return _message;
        }

        public string GetBasicTextMessage()
        {
            return String.Format("{0}|{1}|{2}", _level.ToString().ToUpper().Substring(0, 4), _eventDt.ToString(H369Trace.EventTimeFormat), _message);
        }

        public string GetVerboseTextMessage()
        {
            string msg = String.Format("{0}|{1}|{2};", _level.ToString().ToUpper().Substring(0, 4),
                _eventDt.ToString(H369Trace.EventTimeFormat), _message);

            if (_stackFrame != null)
                msg = String.Format("{0} Class={1}; Method={2}; Line={3};",
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

    public static class H369Trace
    {
        static H369Trace()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Remove("Default");
        }

        public static string EventTimeFormat { get { return "yyyy-MM-ddTHH:mm:ss.fff"; } }

        public static void Add(TraceListener listener)
        {
            Trace.Listeners.Add(listener);
        }

        public static void Remove(string name)
        {
            Trace.Listeners.Remove(name);
        }

        public static void Write(H369TraceData traceData)
        {
            Trace.Write(traceData);
        }

        public static void WriteLine(H369TraceData traceData)
        {
            Trace.WriteLine(traceData);
        }
    }

    public class MethodLogger : IDisposable
    {
        int _pid;
        string _method;
        Stopwatch _sw;

        public MethodLogger(string method) : this(method, null) {}

        public MethodLogger(string method, Dictionary<string, object> parameters)
        {
            _sw = new Stopwatch();
            _sw.Start();
            _method = method;
            _pid = Process.GetCurrentProcess().Id;

            string msg = String.Format("Method Entry: Name={0}, PID={1}", _method, _pid);

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> kvp in parameters)
                    msg = String.Format("{0}, {1}={2}", msg, kvp.Key, kvp.Value.ToString());
            }

            H369Trace.WriteLine(new H369TraceData(
                TraceLevel.Verbose, 
                msg));
        }        

        void IDisposable.Dispose()
        {
            _sw.Stop();
            H369Trace.WriteLine(new H369TraceData(TraceLevel.Verbose, String.Format("Method Exit: Name={0}, PID={1}, Elapsed={2}", _method, _pid, _sw.ElapsedMilliseconds)));
        }
    }
}
