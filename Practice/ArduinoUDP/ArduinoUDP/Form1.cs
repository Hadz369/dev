using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ArduinoUDP
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Listener thread control variables
        /// </summary>
        Thread udpThread, procThread;
        System.Threading.Timer wdTimer;

        bool _stopListener = true, _stopProcessing = true, _udpListening = false;
        int _txPort = 15702, _rxPort = 15701;

        DateTime _lastRecv = DateTime.Now;

        ArduinoUDP.Database.ArduinoMySQL _db;

        Queue pktQueue = new Queue();

        UInt16 _handle = 1000;

        //Creates a UdpClient for reading incoming data.
        UdpClient udpClient;

        public Form1()
        {
            InitializeComponent();
            _db = ArduinoUDP.Database.ArduinoMySQL.Instance;
            udpClient = new UdpClient(_rxPort);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label_Database.Text = "Connecting";
            bwDatabase.RunWorkerAsync();
            /*
            TimerCallback tcb = new TimerCallback(WatchdogTimerFired);
            wdTimer = new System.Threading.Timer(tcb);
            wdTimer.Change(0, 1000);
             * */
        }

        private void WatchdogTimerFired(object State)
        {
            DateTime now = DateTime.Now;

            SetText(now.ToString("dd/MM/yyyy HH:mm:ss"));

            if (!_stopListener && _lastRecv < now.AddSeconds(-10))
            {
                LogEvent(EventType.Error, "Restarting UDP listener due to no data received for 10 seconds.");
                _lastRecv = now;
                _udpListening = false;
            }
        }

        delegate void SetTextCallback(string Text);

        void SetText(string Text)
        {
            if (textBox_Data.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { Text });
            }
            else
            {
                textBox_Data.Text = Text;
            }
        }

        /// <summary>
        /// Controls the start and stop of the two UDP listeners
        /// used for data and status messaging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Start_Click(object sender, EventArgs e)
        {
            // Check the button text to decide what to do
            if (button_Start.Text == "Start")
            {
                // Flush the client queue before starting
                if (udpClient.Available > 0)
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                    udpClient.Receive(ref ep);
                }

                StartListener();
            }
            else
            {
                StopListener();
            }
        }

        /// <summary>
        /// Starts two UDP listeners. One for direct responses to sent messages and one
        /// to receive status broadcast messages
        /// </summary>
        private void StartListener()
        {
            _stopListener = false;
            _stopProcessing = false;

            if (udpThread == null)
            {
//                udpThread = new Thread(new ThreadStart(ReceiveMessages)); 
                udpThread = new Thread(new ThreadStart(ReceiveData)); 
                udpThread.Start();
            }

            if (procThread == null)
            {
                procThread = new Thread(new ThreadStart(ProcessQueue));
                procThread.Start();
            }

            ToggleButtons(true);
        }

        /// <summary>
        /// Stop both UDP listeners
        /// </summary>
        private void StopListener()
        {
            _stopListener = true;
            _stopProcessing = true;

            _udpListening = false;
            
            if (udpThread != null && udpThread.ThreadState != ThreadState.Unstarted)
            {
                LogEvent(EventType.Info, "Stopping UDP Thread");
                udpThread.Join(); // Wait for the thread to close
                LogEvent(EventType.Info, "UDP Thread Stopped");
            }

            if (procThread != null && procThread.ThreadState != ThreadState.Unstarted)
            {
                LogEvent(EventType.Info, "Stopping Queue Processing Thread");
                procThread.Join(); // Wait for the thread to close
                LogEvent(EventType.Info, "Queue Processing Thread Stopped");
            }

            ToggleButtons(false);

            udpThread = null;
            procThread = null;
        }

        /// <summary>
        /// Toggle the start/stop button
        /// </summary>
        /// <param name="IsRunning"></param>
        private void ToggleButtons(bool IsRunning)
        {
            if (IsRunning)
            {
                button_Start.Text = "Stop";
                button_Send.Enabled = true;
                label_Status.Text = "Listening";
            }
            else
            {
                button_Start.Text = "Start";
                button_Send.Enabled = false;
                label_Status.Text = "Stopped";
            }
        }

        public void ReceiveData()
        {
            //Creates an IPEndPoint to record the IP Address and port number of the sender. 
            // The IPEndPoint will allow you to read datagrams sent from any source.
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);

            while (!_stopProcessing) // Run until the stop flag is set
            {
                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref ep);

                PacketQueueData d = new PacketQueueData(ep, DateTime.Now, receiveBytes);
                pktQueue.Enqueue(d);
                Thread.Sleep(1);
            }
        }

        public void ProcessQueue()
        {
            while (!_stopProcessing)
            {
                while (pktQueue.Count > 0)
                {
                    PacketQueueData d = (PacketQueueData)pktQueue.Dequeue();

                    string bytes = "";
                    foreach (byte b in d.Data)
                    {
                        bytes += ((int)b).ToString("X");
                    }

                    LogEvent(EventType.Receive, ByteArrayToHexString(d.Data), d.Data.Length);

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
                str += ((int)b).ToString("X") + " ";
            }

            return str.Trim();
        }

        private void ProcessPacket(IPEndPoint EndPoint, byte[] Bytes)
        {
            try
            {
                if (Bytes != null && Bytes.Length > 1)
                {
                    switch (Bytes[1])
                    {
                        case (byte)PacketType.Signon:
                            ProcessSignOn(EndPoint);
                            break;
                        case (byte)PacketType.EnergyMeter:
                            ProcessEnergyMeter(EndPoint, Bytes);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogEvent(EventType.Error, "Unhandled error during ProcessPacket call: Msg=" + ex.Message);
            }
        }

        private void ProcessSignOn(IPEndPoint endPoint)
        {
            byte[] bytes = new byte[4];
            
            bytes[0] = (byte)PacketType.ACK;
            bytes[1] = (byte)PacketType.Signon;
            byte[] handle = BitConverter.GetBytes(GenerateHandle());
            bytes[2] = handle[0];
            bytes[3] = handle[1];
            
            byte[] mac = ValidateMACAddress(endPoint);

            SendPacket(endPoint.Address, bytes);
        }

        private UInt16 GenerateHandle()
        {
            return _handle++;
        }

        private void ProcessEnergyMeter(IPEndPoint EndPoint, byte[] Data)
        {
            /*
            SetC1Gauge1(0, (float)BitConverter.ToInt32(Data, 12) / 1000);
           
            if (Data[4] == 1) SetC1Gauge2(0, (float)BitConverter.ToInt32(Data, 16) / 1000);
            if (Data[4] == 2) SetC1Gauge3(0, (float)BitConverter.ToInt32(Data, 16) / 1000);
            if (Data[4] == 3) SetC1Gauge4(0, (float)BitConverter.ToInt32(Data, 16) / 1000);
            if (Data[4] == 4) SetC1Gauge5(0, (float)BitConverter.ToInt32(Data, 16) / 1000);
            if (Data[4] == 5) SetC1Gauge6(0, (float)BitConverter.ToInt32(Data, 16) / 1000);
            */
            byte[] bytes = { (byte)PacketType.ACK, Data[3], Data[4], Data[5]};
            SendPacket(EndPoint.Address, bytes);
        }

        delegate void SetC1Gauge1Callback(int Index, float Value);

        void SetC1Gauge1(int Index, float Value)
        {
            if (c1Gauge1.InvokeRequired)
            {
                SetC1Gauge1Callback d = new SetC1Gauge1Callback(SetC1Gauge1);
                this.Invoke(d, new object[] { Index, Value });
            }
            else
            {
                c1Gauge1.Gauges[Index].Pointer.Value = Value;
            }
        }

        delegate void SetC1Gauge2Callback(int Index, float Value);

        void SetC1Gauge2(int Index, float Value)
        {
            if (c1Gauge2.InvokeRequired)
            {
                SetC1Gauge2Callback d = new SetC1Gauge2Callback(SetC1Gauge2);
                this.Invoke(d, new object[] { Index, Value });
            }
            else
            {
                c1Gauge2.Gauges[Index].Pointer.Value = Value;
            }
        }

        delegate void SetC1Gauge3Callback(int Index, float Value);

        void SetC1Gauge3(int Index, float Value)
        {
            if (c1Gauge3.InvokeRequired)
            {
                SetC1Gauge3Callback d = new SetC1Gauge3Callback(SetC1Gauge3);
                this.Invoke(d, new object[] { Index, Value });
            }
            else
            {
                c1Gauge3.Gauges[Index].Pointer.Value = Value;
            }
        }

        delegate void SetC1Gauge4Callback(int Index, float Value);

        void SetC1Gauge4(int Index, float Value)
        {
            if (c1Gauge4.InvokeRequired)
            {
                SetC1Gauge4Callback d = new SetC1Gauge4Callback(SetC1Gauge4);
                this.Invoke(d, new object[] { Index, Value });
            }
            else
            {
                c1Gauge4.Gauges[Index].Pointer.Value = Value;
            }
        }

        delegate void SetC1Gauge5Callback(int Index, float Value);

        void SetC1Gauge5(int Index, float Value)
        {
            if (c1Gauge5.InvokeRequired)
            {
                SetC1Gauge5Callback d = new SetC1Gauge5Callback(SetC1Gauge5);
                this.Invoke(d, new object[] { Index, Value });
            }
            else
            {
                c1Gauge5.Gauges[Index].Pointer.Value = Value;
            }
        }

        delegate void SetC1Gauge6Callback(int Index, float Value);

        void SetC1Gauge6(int Index, float Value)
        {
            if (c1Gauge6.InvokeRequired)
            {
                SetC1Gauge6Callback d = new SetC1Gauge6Callback(SetC1Gauge6);
                this.Invoke(d, new object[] { Index, Value });
            }
            else
            {
                c1Gauge6.Gauges[Index].Pointer.Value = Value;
            }
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        static byte[] ValidateMACAddress(IPEndPoint EndPoint)
        {
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;

            if (SendARP(BitConverter.ToInt32(EndPoint.Address.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) != 0)
                return null; // The SendARP call failed

            return macAddr;
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
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                    IPEndPoint ep = new IPEndPoint(Address, _txPort);

                    s.SendTo(PacketData, ep);
                    LogEvent(EventType.Send, ByteArrayToHexString(PacketData));
                }
                catch (Exception ex)
                {
                    LogEvent(EventType.Error, "Error sending message: Msg=" + ex.Message);
                }
            }
        }

        /// <summary>
        /// LogEvent callback used for thread safety
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Message"></param>
        delegate void LogEventCallback(EventType Type, string Message, int Length);

        /// <summary>
        /// Log an event to the list view
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Message"></param>
        private void LogEvent(EventType Type, string Message)
        {
            LogEvent(Type, Message, 0);
        }

        /// <summary>
        /// Log an event to the list view
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Message"></param>
        /// <param name="Length"></param>
        private void LogEvent(EventType Type, string Message, int Length)
        {
            if (listView_Messages.InvokeRequired)
            {
                LogEventCallback cb = new LogEventCallback(LogEvent);
                Invoke(cb, new object[] { Type, Message, Length });
            }
            else
            {
                try
                {
                    listView_Messages.BeginUpdate();

                    if (listView_Messages.Items.Count > Convert.ToInt32(textBox_MaxLog.Text))
                    {
                        for (int x = 0; x < listView_Messages.Items.Count - Convert.ToInt32(textBox_MaxLog.Text); x++)
                            listView_Messages.Items.Remove(listView_Messages.Items[0]);
                    }

                    ListViewItem i = new ListViewItem(Type.ToString());
                    i.SubItems.Add(Length.ToString());
                    i.SubItems.Add(Message);
                    switch (Type)
                    {
                        case EventType.Error:
                            i.ForeColor = Color.Red;
                            break;
                        case EventType.Receive:
                            i.ForeColor = Color.Green;
                            break;
                        case EventType.Send:
                            i.ForeColor = Color.Blue;
                            break;
                        default:
                            i.ForeColor = Color.Black;
                            break;
                    }

                    listView_Messages.Items.Add(i);
                    i.EnsureVisible();

                    listView_Messages.EndUpdate();
                }
                catch (Exception ex)
                {
                    LogEvent(EventType.Error, "Unhandled error during LogEvent call: Msg=" + ex.Message);
                }
            }
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            if (textBox_Data.Text != "")
            {
                IPAddress ip = IPAddress.Parse(textBox_Address.Text);
                SendPacket(ip, textBox_Data.Text);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopListener();
            Application.Exit();
        }

        // Create a switch mask and set it to 0
        byte Mask = (byte)RelayMask.None;

        private void button1_Click(object sender, EventArgs e)
        {            
            IPAddress ip = IPAddress.Parse(textBox_Address.Text);
            SendPacket(ip, new byte[] { SwitchOn(Mask, RelayMask.Relay1)});
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(textBox_Address.Text);
            SendPacket(ip, new byte[] { SwitchOff(Mask, RelayMask.Relay1) });
        }

        private byte SwitchOn(byte Mask, RelayMask Switch)
        {
            return (byte)(Mask | (byte)Switch);
        }

        private byte SwitchOff(byte Mask, RelayMask Switch)
        {
            if ((Mask & (byte)Switch) == 1) 
                Mask = (byte)(Mask ^ (byte)Switch); // Toggle the switch

            return Mask;
        }

        private void createTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string cstr = Properties.Settings.Default.
        }

        private void bwDatabase_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_db.Connected) label_Database.Text = "Connected";
            else label_Database.Text = "Not connected";
        }

        private void bwDatabase_DoWork(object sender, DoWorkEventArgs e)
        {
            _db.Connect(Properties.Settings.Default.ArduinoDB);
        }

        private void typeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formDataViewer f = new formDataViewer();
            f.LoadGrid(_db.GetList("type"));
            f.ShowDialog();
        }

        private void codeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formDataViewer f = new formDataViewer();
            f.LoadGrid(_db.GetList("code"));
            f.ShowDialog();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            SendBroadcast(new byte[] { 0x44, 0x45, 0x46 });
        }
    }

    class DeviceInfo
    {
        public DeviceInfo(IPEndPoint EndPoint)
        {
        }
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
        public UdpClient u;
        public IPEndPoint e;
    }

    enum PacketType
    {
        ACK = 0x06,
        NAK = 0x15,
        Signon = 0xC8,
        Status = 0xC9,
        Sensor = 0xCA,
        EnergyMeter = 0xCB
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
