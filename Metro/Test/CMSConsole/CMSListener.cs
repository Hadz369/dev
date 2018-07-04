sing System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;

namespace CMS
{

    public class CMSListener
    {
        SerialPort _port = null;

        public CMSListener(CMSPortSettings settings)
        {
            _port = new SerialPort(settings.PortName, settings.BaudRate, settings.Parity, settings.DataBits, settings.StopBits);
            _port.DataReceived += _port_DataReceived;
        }

        void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
