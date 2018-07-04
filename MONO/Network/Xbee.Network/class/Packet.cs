using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbee.Network
{
    public static partial class Constant
    {
        public static byte  MINPACKETLEN = 16;
        public static byte  MAXPACKETLEN = 128;
        public static byte  STARTBYTE    = 0x7E;
    }

    public enum PacketType
    {
        Sensor = 0x92,
    }

    public interface IPacket
    {
    }

    public class PacketQueue
    {
        Queue<PacketQueueItem> _queue;
        AverageCalculator _calc;
        int _totPackets = 0;
        Dictionary<PacketType, int> _pktCount = new Dictionary<PacketType, int>();

        public PacketQueue()
        {
            _queue = new Queue<PacketQueueItem>();
            _calc = new AverageCalculator(20);
        }

        public int TotalPackets { get { return _totPackets; } }

        public double AverageWaitTime { get { return _calc.Average; } }

        public Dictionary<PacketType, int> PacketCount { get { return _pktCount; } }

        public void Enqueue(IPacket packet)
        {
            PacketQueueItem i = new PacketQueueItem(packet);
            _queue.Enqueue(i);
        }

        public IPacket Dequeue()
        {
            PacketQueueItem i = _queue.Dequeue();

            if (i != null)
            {
                TimeSpan ts = DateTime.Now.Subtract(i.Received);
                _calc.AddValue(ts.TotalMilliseconds);
            }

            return i.Packet;
        }

        public int Count { get { return _queue.Count; } }

        public IPacket Peek { get { return DoPeek(); } }

        private IPacket DoPeek()
        {
            IPacket packet = null;

            PacketQueueItem i = _queue.Peek();
            if (i != null)
            {
                packet = i.Packet;
            }

            return packet;
        }
    }

    class PacketQueueItem
    {
        IPacket _packet;
        DateTime _received;

        public PacketQueueItem(IPacket packet)
        {
            _packet = packet;
            _received = DateTime.Now;
        }

        public DateTime Received { get { return _received; } }

        public IPacket Packet { get { return _packet; } }
    }

    public class AverageCalculator : List<double>
    {
        int _ll = 0;
        Object _locker = new Object();

        public AverageCalculator(int listLength)
        {
            _ll = listLength;
        }

        public void AddValue(double value)
        {
            this.Add(value);

            int z = this.Count - _ll;

            if (z > 0)
            {
                for (int x = 0; x < z; x++)
                    this.RemoveAt(0);
            }
        }

        public double Average { get { return CalculateAverage(); } }

        double CalculateAverage()
        {
            double avg = 0, tot = 0;
            int count = this.Count;

            if (count > 0)
            {
                lock (_locker)
                {
                    foreach (double l in this)
                    {
                        tot += l;
                    }

                    avg = tot / count;
                }
            }

            return avg;
        }
    }

    public static class PacketValidator
    {
        public static bool ValidateCRC(byte[] bytes)
        {
            bool ok = false;
            byte sum = 0;

            byte crc = bytes[bytes.Length-1];

            for (int x = 0; x < bytes.Length-1; x++)
            {
                sum += (byte)bytes[x];
            }

            sum &= 0xFF;
            sum = (byte)(0xFF - sum);

            if (sum == crc)
                ok = true;

            return ok;
        }


        public static IPacket FormatPacket(byte[] bytes)
        {
            IPacket _packet = null;

            PacketType type = (PacketType)bytes[0];

            switch (type)
            {
                case PacketType.Sensor:
                    _packet = new XbeeSensorPacket(bytes);
                    break;
                default:
                    break;
            }
            return _packet;
        }
    }

    public class XbeeSensorPacket : IPacket
    {
        byte[] _bytes = null;
        Int64 _srcAddress = 0;
        Int16 _netAddress = 0;
        byte _rcvOptions = 0, _numSamples = 0;
        Int16 _digiMask = 0, _digiSamples = 0;
        byte _analogMask, _frameType, _checksum;
        public Dictionary<byte, Int16> _analogSamples = new Dictionary<byte, short>();

        public XbeeSensorPacket(byte[] bytes)
        {
            _frameType  = bytes[0];
            _srcAddress = Common.BytesToInt64(bytes, XbeeConst.XB92_DEVADDRESS);
            _netAddress = Common.BytesToInt16(bytes, XbeeConst.XB92_NETADDRESS);
            _rcvOptions = bytes[XbeeConst.XB92_RCVOPTIONS];
            _numSamples = bytes[XbeeConst.XB92_NUMSAMPLES];
            _digiMask   = Common.BytesToInt16(new byte[] { bytes[XbeeConst.XB92_DIGITALMASK+1], bytes[XbeeConst.XB92_DIGITALMASK] }, 0);
            _analogMask = bytes[XbeeConst.XB92_ANALOGMASK];

            if (_digiMask > 0)
            {
                _digiSamples = Common.BytesToInt16(bytes, XbeeConst.XB92_SAMPLES);
                GetAnalogSamples(bytes, XbeeConst.XB92_SAMPLES + 2);
            }
            else
            {
                GetAnalogSamples(bytes, XbeeConst.XB92_SAMPLES);
            }
        }

        public byte[] Bytes { get { return _bytes; } }

        public byte FrameType { get { return _frameType; } }
        public byte Checksum { get { return _checksum; } }
        public Int64 SourceAddress { get { return _srcAddress; } }
        public Int16 NetworkAddress { get { return _netAddress; } }

        public byte ReceiveOptions { get { return _rcvOptions; } }
        public byte NumSamples { get { return _numSamples; } }

        // Use if (DigitalMask & XbeeDigitalMask.<Pin>) > 0) to check if sampled
        public Int16 DigitalMask { get { return _digiMask; } }

        // Use if (DigitalSamples & XbeeDigitalMask.<Pin>) > 0) to get sample value
        public Int16 DigitalSamples { get { return _digiSamples; } }

        // Use if (AnalogMask & XbeeAnalogMask.<Pin>) > 0) to check if sampled
        public byte AnalogMask { get { return _analogMask; } }

        // Use AnalogSamples[XbeeAnalogMask.<Pin>] to get sample value
        public Dictionary<byte, Int16> AnalogSamples { get { return _analogSamples; } }

        // The array includes the checksum so subtract 2
        [Obsolete]
        public byte AnalogRead { get { return _bytes[_bytes.Length-2]; } }

        public void GetAnalogSamples(byte[] bytes, int startPos)
        {
            if (_analogMask > 0)
            {
                int bits = _analogMask;
                int a = startPos, b = startPos + 1;

                for (int x = 0; x < 8; x++)
                {
                    // Check the value if it is A0-A4 or the supply voltage
                    if (x < 4 || x == 7)
                    {
                        if ((bits & 0x01) > 0)
                        {
                            Int16 sample = (Int16)(bytes[a] * 0xFF + bytes[b]);
                           
                            _analogSamples.Add((byte)x, sample);

                            // Move to the next field
                            a = b + 1;
                            b = a + 1;
                        }
                    }
                    // Move to the next bit
                    bits = bits >> 1;
                }
            }
        }
    }

    public static class XbeeAnalogMask
    {
        public static byte A0 = 0x01;  // Analog 0
        public static byte A1 = 0x02;  // Analog 1
        public static byte A2 = 0x04;  // Analog 2
        public static byte A3 = 0x08;  // Analog 3 
        public static byte SV = 0x80;  // Supply Voltage
    }

    public static class XbeeDigitalMask
    {
        public static Int16 D0  = 0x01;    // Digital 0
        public static Int16 D1  = 0x02;    // Digital 1
        public static Int16 D2  = 0x04;    // Digital 2
        public static Int16 D3  = 0x08;    // Digital 3 
        public static Int16 D4  = 0x10;    // Digital 4
        public static Int16 D5  = 0x20;    // Digital 5
        public static Int16 D6  = 0x40;    // Digital 6
        public static Int16 D7  = 0x80;    // Digital 7
        public static Int16 D10 = 0x400;   // Digital 10
        public static Int16 D11 = 0x800;   // Digital 11
        public static Int16 D12 = 0x1000;  // Digital 12
    }
}
