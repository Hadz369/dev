using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace TempLogger
{
    class Program
    {
        static SerialPort _port;
        static bool _continue = true;
        static Thread _portListener, _queueListener;
        static Queue<string> _queue;
        static string _logFile;

        static void Main(string[] args)
        {
            _logFile = "data.log";

            _queue = new Queue<string>();

            _port = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);
            _port.Open();

            _portListener = new Thread(new ThreadStart(PortListener));
            _portListener.Start();

            _queueListener = new Thread(new ThreadStart(QueueListener));
            _queueListener.Start();

            Console.ReadLine();
            _continue = false;

            _portListener.Join();
            _queueListener.Join();

            _port.Close();
        }

        static void PortListener()
        {
            while (_continue)
            {
                try
                {
                    string message = _port.ReadLine();

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Properties.Settings.Default.LogFile, true))
                        sw.WriteLine(message);

                    if (message.Length > 6 && message.Substring(0, 6) == "[DATA]")
                        _queue.Enqueue(message.Substring(7));
                    else
                        Console.WriteLine(message);
                }
                catch (TimeoutException) { }

                Thread.Sleep(100);
            }
        }

        static void QueueListener()
        {
            while (_continue)
            {
                for (int x = 0; x < _queue.Count; x++)
                {
                    string msg = _queue.Dequeue();

                    try
                    {
                        Data data = new Data(msg);

                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Properties.Settings.Default.DataFile, true))
                            sw.WriteLine(msg);

                        Console.WriteLine(String.Concat("[DATA] ", data.ToString()));
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine(String.Concat("[ERRO] ", msg));
                    }
                }

                Thread.Sleep(100);
            }
        }
    }

    class Data
    {
        public Data(string data)
        {
            string[] fields = data.Split(new char[] { ',' } );

            if (fields.Length != 6) throw new ArgumentException("[ERRO] Invalid data packet received");

            ExecutionState = Convert.ToInt16(fields[0]);
            CompressorState = fields[1] == "0" ? false : true;
            Sensor1Temp = Convert.ToDecimal(fields[2]);
            Sensor2Temp = Convert.ToDecimal(fields[3]);
            SetPointReached = fields[4] == "0" ? false : true;
            Countdown = Convert.ToInt16(fields[5]);
        }

        public Int16   ExecutionState  { get; private set; }
        public bool    CompressorState { get; private set; }
        public decimal Sensor1Temp     { get; private set; }
        public decimal Sensor2Temp     { get; private set; }
        public bool    SetPointReached { get; private set; }
        public Int16   Countdown       { get; private set; }

        public override string ToString()
        {
            return String.Format("ExexState={0}, CompState={1}, Temp1={2}, Temp2={3}, SetPoint={4}, Countdown={5}",
                ExecutionState, CompressorState, Sensor1Temp, Sensor2Temp, SetPointReached, Countdown);
        }
    }
}
