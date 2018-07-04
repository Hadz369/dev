using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Xbee.Network
{
    public delegate void DataReceivedHandler(string data);
    public delegate void DataDiscardedHandler(string data);

    public enum DeviceType
    {
        XBeeS2
    }

    public class SerialPortManager
    {
        SerialPort _port = null;
        Object locker = new Object();
        List<byte> _residual = new List<byte>();
        bool _close = false;
        DeviceType _devType;

        public event DataReceivedHandler DataReceived;
        public event DataDiscardedHandler DataDiscarded;

        PacketQueue _packets = new PacketQueue();

        public SerialPortManager(DeviceType devType, string portName, Int32 baudRate, System.IO.Ports.Parity parity, int dataBits, System.IO.Ports.StopBits stopBits)
        {
            _devType = devType;
            _port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        }

        public DeviceType DeviceType { get { return _devType; } }

        public PacketQueue PacketQueue { get { return _packets; } }

        public void Open()
        {
            try
            {
                _close = false;
                _port.Open();

                while (!_close)
                {
                    ProcessBuffer();
                    Thread.Sleep(100);
                }

                _port.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening serial port " + _port.PortName, ex);
            }
        }

        public void Close()
        {
            _close = true;
        }

        private void ProcessBuffer()
        {
            int dataLen;
            byte[] bytes, part;
            byte type;

            bytes = GetBuffer();
            dataLen = bytes.Length;

            if (dataLen < Constant.MINPACKETLEN)
            {
                AddResidual(bytes, 0);
            }
            else
            {
                int x = 0, staPos;
                Int16 plen;
                string discard = "";

                while (x < dataLen)
                {
                    // If a start character is found, read the length and then the full packet.
                    // If the remaining data length is less than the packet size, put it into the
                    // residual buffer for the next pass.
                    if (bytes[x] == Constant.STARTBYTE)
                    {
                        staPos = x;

                        x++; // Move to the next byte and get the two byte packet length
                        part = new byte[2];
                        Buffer.BlockCopy(bytes, x, part, 0, 2);
                        plen = GetLength(part);

                        // If there are not enough characters to satisfy the packet length, add all the 
                        // bytes from the last packet identifier to the residual for the next pass
                        if (dataLen - staPos < plen + 3)
                        {
                            AddResidual(bytes, staPos);
                            break;
                        }
                        else
                        {
                            x += 2; //Move two bytes to the frame type
                            type = bytes[x];

                            part = new byte[plen];
                            Buffer.BlockCopy(bytes, x, part, 0, plen);

                            x += plen; // Move to the end of the packet

                            if (DataReceived != null)
                                DataReceived(ByteArrayToHexString(part, 0, part.Length));

                            if (PacketValidator.ValidateCRC(part))
                            {
                                IPacket packet = PacketValidator.FormatPacket(part);
                                _packets.Enqueue(packet);
                            }
                            else
                            {
                                // Add all chars after the last start 0x7E byte since the packet is invalid
                                // This will force the next pass to read individual bytes until it finds the 
                                // next packet start character
                                AddResidual(bytes, staPos + 1);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (_residual.Count == 0)
                        {
                            // Discard bytes after a packet until a start byte is found
                            discard = String.Concat(discard, discard == "" ? "" : " ", bytes[x].ToString("X2"));
                        }
                        else
                        {
                            _residual.Add(bytes[x]);
                        }
                    }
                    x++;
                }

                if (discard != "" && DataDiscarded != null)
                {
                    DataDiscarded(discard);
                }
            }
        }
        
        /// <summary>
        /// Convert the first two bytes of a supplied byte array to an Int16
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        Int16 GetLength(byte[] bytes)
        {
            Int16 len;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            len = BitConverter.ToInt16(bytes, 0);

            len++; // Add 1 to include the CRC

            return len;
        }

        // Add a byte array to the residual starting at the specified position
        void AddResidual(byte[] bytes, int startpos)
        {
            for (int x = startpos; x < bytes.Length - startpos; x++)
                _residual.Add(bytes[x]);
        }

        /// <summary>
        /// Read data from the port buffer and append to any residual value left from the previous pass
        /// </summary>
        /// <returns></returns>
        byte[] GetBuffer()
        {
            byte[] buf, bytes;

            int buflen = _port.BytesToRead;

            // Only read the max packet length
            if (buflen > Constant.MAXPACKETLEN) buflen = Constant.MAXPACKETLEN;

            buf = new byte[buflen];
            _port.Read(buf, 0, buflen);

            // Create a new array to hold the residual and the buffer
            int totlen = _residual.Count + buflen;
            bytes = new byte[totlen];

            // If there is residual data, add it to the start of the array
            if (_residual.Count > 0)
            {
                byte[] resbuf = _residual.ToArray();
                Buffer.BlockCopy(resbuf, 0, bytes, 0, resbuf.Length);
            }

            // Add the buffer data
            Buffer.BlockCopy(buf, 0, bytes, _residual.Count, buflen);

            // Clear the residual array
            _residual.Clear();

            return bytes;
        }

        /// <summary>
        /// Convert a byte array to a space delimited string of hex bytes (AA AB AC 00 01....)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startpos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string ByteArrayToHexString(byte[] bytes, int startpos, int length)
        {
            string str = "";

            for (int x = startpos; x < length; x++ )
            {
                if (x > bytes.Length - 1) break;
                    str = String.Concat(str, str == "" ? "" : " ", bytes[x].ToString("X2"));
            }

            return str;
        }
    }
}
