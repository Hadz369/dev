using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ArduinoUDP
{
	class DeviceManager
	{
        Int32 _handle = 1000;
        System.Windows.Forms.ListView _listView;

        DataTable _dt = new DataTable();

        public DeviceManager(System.Windows.Forms.ListView listView)
        {
            _listView = listView;
            Initialise();
        }

        private void Initialise()
        {
        }

        public Int32 GetHandle(byte[] macAddress, byte[] ipAddress)
        {
            return _handle++;
        }
	}

    class DeviceDetail
    {
        byte[] _mac, _ip;
        int _tx = 0, _rx = 0;
        Int32 _handle;
        DeviceStatus _status;

        public DeviceDetail(byte[] macAddress, byte[] ipAddress, Int32 handle)
        {
            _mac = macAddress;
            _ip = ipAddress;
            _handle = handle;
        }

        public byte[] macAddress { get { return _mac; } }

        public Int32 handle { get { return _handle; } }

        public byte[] ipAddress
        {
            get { return _ip; }
            set { _ip = value; }
        }

        public DeviceStatus status
        {
            get { return _status; }
            set { _status = value; }
        }
    }

    enum DeviceStatus
    {
        Offline,
        Online        
    }
}
