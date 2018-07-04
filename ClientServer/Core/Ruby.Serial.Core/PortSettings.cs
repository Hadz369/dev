using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Ruby.Serial
{
    public class SerialPortSettings
    {
        string _portName;
        int _baudRate, _dataBits, _readBufferSize = 262144;
        Parity _parity;
        StopBits _stopBits;
        Encoding _encoding = Encoding.UTF8;

        public SerialPortSettings(string portname, int baudrate, Parity parity, int databits, StopBits stopbits)
        {
            _portName = portname;
            _baudRate = baudrate;
            _parity = parity;
            _dataBits = databits;
            _stopBits = stopbits;
        }

        public string PortName { get { return _portName; } }
        public int BaudRate { get { return _baudRate; } }
        public Parity Parity { get { return _parity; } }
        public int DataBits { get { return _dataBits; } }
        public StopBits StopBits { get { return _stopBits; } }

        public int ReadBufferSize { get { return _readBufferSize; } }
        public Encoding Encoding { get { return _encoding; } }
    }
}
