using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;

using HS;

namespace HS.Network
{
    public class NetworkHandler : IDisposable
    {
        Thread udpThread, procThread;
        Timer wdTimer, pollTimer;

        bool _stopUdpListener = false, _stopPacketManager = false;
        int _txPort = 15702, _rxPort = 15701;

        DateTime _lastRecv = DateTime.Now, _lastTimerFired = DateTime.Parse("01/01/1980");

        Queue pktQueue = new Queue();

        UInt16 _handle = 1000;

        UdpClient _udpClient;

        public NetworkHandler()
        {
            _udpClient = new UdpClient(_rxPort);
            Tracer.Info("UDP listener created on port " + _rxPort.ToString());
        }

        private void StartPollTimer()
        {
            Tracer.Info("Poll timer starting");

            if (pollTimer == null)
                pollTimer = new Timer(new TimerCallback(PollTimerFired));

            pollTimer.Change(10000, 20000);
        }

        private void StopPollTimer()
        {
            pollTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void PollTimerFired(object state)
        {
            Tracer.Debug("Sending poll");
            byte[] b = new byte[] { 0x01, 0xCC };
            SendBroadcast(b);
        }

        private void StartWatchDogTimer()
        {
            Tracer.Info("Watchdog timer starting");
            
            if (wdTimer == null)
                wdTimer = new Timer(new TimerCallback(WatchdogTimerFired));

            wdTimer.Change(1000, 1000);
            _lastRecv = DateTime.Now;
        }

        private void StopWatchDogTimer()
        {
            Tracer.Info("Watchdog timer stopped");
            wdTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void WatchdogTimerFired(object State)
        {
            DateTime now = DateTime.Now;

            _lastTimerFired = now;

            if (!_stopUdpListener && _lastRecv < now.AddSeconds(-30))
            {
                StopWatchDogTimer();

                Tracer.Warning("Restarting UDP listener due to no data received for 30 seconds.");

                Stop(true, false);
                Thread.Sleep(100); // Just in case.
                Start();
            }
        }

        public DateTime LastTimerFired { get { return _lastTimerFired; } }

        public void Start()
        {
            if (udpThread == null)
            {
                Tracer.Info("Starting UDP listener");

                udpThread = new Thread(new ThreadStart(ReceiveData));
                udpThread.Start();
            }

            if (procThread == null)
            {
                Tracer.Info("Starting packet manager");

                procThread = new Thread(new ThreadStart(ProcessQueue));
                procThread.Start();
            }

            StartWatchDogTimer();
            StartPollTimer();
        }

        public void Stop()
        {
            StopPollTimer();
            StopWatchDogTimer();
            Stop(true, true);
        }

        private void Stop(bool stopUdpListener, bool stopPacketManager)
        {
            _stopUdpListener = stopUdpListener;
            _stopPacketManager = stopPacketManager;

            if (_stopUdpListener && udpThread != null && udpThread.ThreadState != ThreadState.Unstarted)
            {
                Tracer.Info("Stopping UDP listener");
                udpThread.Join(5000); // Wait for the thread to close
                udpThread.Abort();

                Tracer.Info("UDP listener Stopped");
                udpThread = null;                
            }

            if (_stopPacketManager && procThread != null && procThread.ThreadState != ThreadState.Unstarted)
            {
                Tracer.Info("Stopping packet manager");
                procThread.Join(5000); // Wait for the thread to close
                procThread.Abort();

                Tracer.Info("Packet manager stopped");
                procThread = null;
            }
        }

        public void ReceiveData()
        {
            //Creates an IPEndPoint to record the IP Address and port number of the sender. 
            // The IPEndPoint will allow you to read datagrams sent from any source.
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            
            _stopUdpListener = false;

            //byte[] rbuf;

            while (!_stopUdpListener) // Run until the stop flag is set
            {
                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = _udpClient.Receive(ref ep);

                _lastRecv = DateTime.Now;
                Tracer.Debug(String.Format("RECV: Length={0}, Data={1}", receiveBytes.Length, ByteArrayToHexString(receiveBytes)));

                PacketQueueData d = new PacketQueueData(ep, DateTime.Now, receiveBytes);
                pktQueue.Enqueue(d);
                Thread.Sleep(1);
            }
        }

        private void ProcessQueue()
        {
            _stopPacketManager = false;

            while (!_stopPacketManager)
            {
                while (pktQueue.Count > 0)
                {
                    PacketQueueData d = (PacketQueueData)pktQueue.Dequeue();

                    ProcessPacket(d.EndPoint, d.Data);
                }
                Thread.Sleep(1);
            }
        }
	

        private string ByteArrayToHexString(byte[] Bytes)
        {
            string str = "";

            foreach (byte b in Bytes)
            {
                str += ((int)b).ToString("X2") + " ";
            }

            return str.Trim();
        }

        private void ProcessPacket(IPEndPoint EndPoint, byte[] Bytes)
        {
            try
            {
                if (Bytes != null && Bytes.Length > 1)
                {
                    Tracer.Debug("Processing packet:" + Bytes.ToString());

					byte[] header = new byte[12];
					Buffer.BlockCopy(Bytes, 0, header, 0, 12);

                    PacketType pt = (PacketType)Bytes[1];
                    //IPacket pkt = null;

                    switch (pt)
                    {
                        case PacketType.PowerSummary:
                            ProcessPowerSummary(EndPoint, ref Bytes);
                            break;
                        case PacketType.Signon:
						    Tracer.Debug("Signon packet");
                            ProcessSignOn(EndPoint);
                            break;
                        case PacketType.Command:
                            Tracer.Debug("Command packet");
                            ProcessCommand(EndPoint, Bytes);
                            break;
                        case PacketType.EnergyMeter:
                            Tracer.Debug("Energy packet");
                            ProcessEnergyMeter(EndPoint, Bytes);
                            break;
                        default:
                            Tracer.Warning("Invalid packet received");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.Error("Unhandled error during ProcessPacket call: Msg=" + ex.Message, ex);
            }
        }

		private PacketType ProcessHeader(byte[] header)
		{
			return PacketType.ACK;
		}
		
        private void ProcessSignOn(IPEndPoint endPoint)
        {
            byte[] bytes = new byte[4];

            bytes[0] = (byte)PacketType.ACK;
            bytes[1] = (byte)PacketType.Signon;
            byte[] handle = BitConverter.GetBytes(GenerateHandle());
            bytes[2] = handle[0];
            bytes[3] = handle[1];

            SendPacket(endPoint.Address, bytes);
        }

		private void ProcessCommand(IPEndPoint endPoint, byte[] Data)
		{
			if (Data[12] == (byte)CommandCode.GetCalibrationData)
			{
				byte[] bytes = new byte[28];
				
				float pin0 = 111.1f;
				float pin1 = 111.1f;
				float pin2 = 111.1f;
				float pin3 = 111.1f;
				float pin4 = 111.1f;
				float pin5 = 241.95f;
				float vphs = 1.1f;
				
				Buffer.BlockCopy(BitConverter.GetBytes(pin0),0,bytes, 0, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(pin1),0,bytes, 4, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(pin2),0,bytes, 8, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(pin3),0,bytes, 12, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(pin4),0,bytes, 16, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(pin5),0,bytes, 20, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(vphs),0,bytes, 24, 4);
				
				SendPacket(endPoint.Address, bytes);				
			}
		}
		
		private UInt16 GenerateHandle()
        {
            return _handle++;
        }

        private void ProcessEnergyMeter(IPEndPoint EndPoint, byte[] Data)
        {
            Tracer.Debug(
                String.Format("" +
                    "Sensor={0}, V={1}, I={2}, RP={3}, " +
			        "AP={4}, PF={5}, SM={6}, EM={7}, PI={8}",
			    Data[12], //sensor
			    (float)(BitConverter.ToInt32(Data, 13)) / 1000, //voltage
			    (float)(BitConverter.ToInt32(Data, 17)) / 1000, //current
			    (float)(BitConverter.ToInt32(Data, 21)) / 1000, //realpower
 			    (float)(BitConverter.ToInt32(Data, 25)) / 1000, //apparentpower
			    (float)(BitConverter.ToInt32(Data, 29)) / 100,  //powerfactor
			    BitConverter.ToInt32(Data, 33),        //samples
			    BitConverter.ToInt32(Data, 37),        //elapsedmillis
			    BitConverter.ToInt32(Data, 41)));       //powerincrement

			byte[] bytes = { (byte)PacketType.ACK, Data[3], Data[4], Data[5] };
            SendPacket(EndPoint.Address, bytes);
        }

        private void ProcessPowerSummary(IPEndPoint endpoint, ref byte[] data)
        {
            Tracer.Debug("Power summary packet"); 
            
            PowerSummary pkt = new PowerSummary(endpoint, ref data);

            /*
            Tracer.Debug(
                String.Format("" +
                    "Sensor={0}, V={1}, I={2}, RP={3}, " +
                    "AP={4}, PF={5}, SM={6}, EM={7}, PI={8}",
                Data[12], //sensor
                (float)(BitConverter.ToInt32(Data, 13)) / 1000, //voltage
                (float)(BitConverter.ToInt32(Data, 17)) / 1000, //current
                (float)(BitConverter.ToInt32(Data, 21)) / 1000, //realpower
                (float)(BitConverter.ToInt32(Data, 25)) / 1000, //apparentpower
                (float)(BitConverter.ToInt32(Data, 29)) / 100,  //powerfactor
                BitConverter.ToInt32(Data, 33),        //samples
                BitConverter.ToInt32(Data, 37),        //elapsedmillis
                BitConverter.ToInt32(Data, 41)));       //powerincrement
            */

            byte[] bytes = { (byte)PacketType.ACK, data[3], data[4], data[5] };

            SendPacket(endpoint.Address, bytes);
        }

        /// <summary>
        /// Send a packet of data to the remote IP
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="PacketData"></param>
        private void SendBroadcast(byte[] PacketData)
        {
            SendPacket(IPAddress.Broadcast, PacketData);
        }

        /// <summary>
        /// Send a packet of data to the remote IP
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="PacketData"></param>
        private void SendPacket(IPAddress Address, string PacketData)
        {
            SendPacket(Address, Encoding.ASCII.GetBytes(PacketData));
        }

        /// <summary>
        /// Send a packet of data to the remote IP
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="PacketData"></param>
        private void SendPacket(IPAddress Address, byte[] PacketData)
        {
            if (PacketData.Length > 0)
            {
                try
                {
                    //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPEndPoint ep = new IPEndPoint(Address, _txPort);

                    Tracer.Debug(String.Format("SEND: Length={0}, Data={1}", PacketData.Length, ByteArrayToHexString(PacketData)));

					_udpClient.Send(PacketData, PacketData.Length, ep);

                    //s.SendTo(PacketData, ep);
                    //LogEvent(EventType.Send, ByteArrayToHexString(PacketData));
                }
                catch (Exception ex)
                {
                    Tracer.Error("Error sending message: Msg=" + ex.Message, ex);
                }
            }
        }

        public void Dispose()
        {
            Stop();

            _udpClient.Close();
            _udpClient = null;

            wdTimer = null;
            pollTimer = null;
            pktQueue = null;
        }
    }

    public class Packet
    {
    }

    [Flags]
    enum RelayMask
    {
        None = 0,
        Relay1 = 1,
        Relay2 = 2,
        Relay3 = 4,
        Relay4 = 8
    }

    enum EventType
    {
        Info,
        Send,
        Receive,
        Error
    }

    class UdpState
    {
        public UdpClient u = null;
        public IPEndPoint e = null;
    }

    class PacketQueueData
    {
        private IPEndPoint _ep;
        private Byte[] _data;
        private DateTime _dt;

        public PacketQueueData(IPEndPoint EndPoint, DateTime ReceiveDT, Byte[] Data)
        {
            _ep = EndPoint;
            _dt = ReceiveDT;
            _data = Data;
        }

        public IPEndPoint EndPoint { get { return _ep; } }
        public DateTime ReceiveDT { get { return _dt; } }
        public Byte[] Data { get { return _data; } }
    }
}