using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ruby.Core;

namespace Ruby.Serial
{
    public class XmlPortBuffer : IPortBuffer
    {
        string _data = "";
        int _absoluteMinPktLength, _minPktLength, _absoluteMaxPktLength, _maxPktLength;
        bool _hasChanges = false;

        public XmlPortBuffer()
        {
            _absoluteMinPktLength = _minPktLength = 11; // <xml></xml>
            _absoluteMaxPktLength = _maxPktLength = 128000;
        }

        Object _locker = new Object();
        
        public int MinPacketLen
        {
            get { return _minPktLength; }
            set { SetMinPacketLength(value); }
        }

        void SetMinPacketLength(int value)
        {
            if (value > _absoluteMinPktLength)
            {
                _minPktLength = value;
            }
            else
            {
                Tracer.Warning(String.Format(
                    "Attempt to set XmlHandler MinPacketLength to {0} which is less than the allowed minimum {1}",
                    value, _absoluteMinPktLength));
            }
        }

        public int MaxPacketLen
        {
            get { return _maxPktLength; }
            set { SetMaxPacketLength(value); }
        }

        void SetMaxPacketLength(int value)
        {
            if (value > _absoluteMaxPktLength)
            {
                _maxPktLength = value;
            }
            else
            {
                Tracer.Warning(String.Format(
                    "Attempt to set XmlHandler MaxPacketLength to {0} which is greater than the allowed maximum {1}",
                    value, _absoluteMaxPktLength));
            }
        }

        public int  Length { get { return _data.Length; } }

        public bool HasData { get { return (Length >= _minPktLength); } }

        public bool HasChanges { get { return _hasChanges; } }

        public void Add(byte[] bytes)
        {
            string x = System.Text.Encoding.UTF8.GetString(bytes);

            lock (_locker)
            {
                _data = String.Concat(_data, x);
                _hasChanges = true;
            }
        }

        public string GetPacket()
        {
            string xml = "";

            if (_data.Length > this.MinPacketLen)
            {
                lock (_locker)
                {
                    int ix = _data.IndexOf("</xml>", 6, StringComparison.InvariantCultureIgnoreCase);

                    if (ix > 0)
                    {
                        xml = _data.Substring(0, ix + 6);
                        _data = _data.Remove(0, xml.Length);
                    }
                }

                if (xml != "")
                {
                    // Find the last xml opening tab and remove anything before it
                    int ix = _data.LastIndexOf("<xml ", 0, StringComparison.InvariantCultureIgnoreCase);
                    
                    if (ix > 0)
                    {
                        xml = xml.Remove(0, ix);
                        Tracer.Warning(String.Concat("Invalid leading characters discarded; Length=", ix));
                    }
                }
            }

            return xml;
        }
    }
}
