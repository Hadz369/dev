using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Object lockObj = new Object();
        BackgroundWorker _bw;
        SerialPortManager pm;

        System.Timers.Timer _timer;

        public Form1()
        {
            InitializeComponent();
            
            pm = new SerialPortManager(DeviceType.XBeeS2, "COM4", 9600, Parity.None, 8, StopBits.One);
            pm.DataReceived += pm_DataReceived;
            pm.DataDiscarded += pm_DataDiscarded;

            _bw = new BackgroundWorker();
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += _bw_DoWork;

            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 1;
            _timer.Start();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (pm.PacketQueue.Count > 0)
            {
                _timer.Stop();

                while (pm.PacketQueue.Count > 0)
                {
                    IPacket p = pm.PacketQueue.Dequeue();
                    SetReading(p);
                    SetTemp(p);
                    SetAvg(pm.PacketQueue.AverageWaitTime);
                    SetVoltage(p);
                }

                _timer.Start();
            }
        }

        void SetReading(IPacket packet)
        {
            if (textBox3.InvokeRequired)
                this.Invoke(new Action<IPacket>(this.SetReading), packet);
            else
            {
                XbeeSensorPacket sp = packet as XbeeSensorPacket;
                if (sp != null)
                {
                    textBox3.Text = sp.AnalogSamples[1].ToString();
                }
            }
        }

        void SetTemp(IPacket packet)
        {
            if (textBox1.InvokeRequired)
                this.Invoke(new Action<IPacket>(this.SetTemp), packet);
            else
            {
                XbeeSensorPacket sp = packet as XbeeSensorPacket;
                if (sp != null)
                {
                    // The DFRobot temp sensor value needs to be divided by 9.31.
                    textBox1.Text = (sp.AnalogSamples[1]/5.3552).ToString("0.0");
                }
            }
        }

        void SetAvg(double value)
        {
            if (textBox2.InvokeRequired)
                this.Invoke(new Action<double>(this.SetAvg), value);
            else
            {
                textBox2.Text = value.ToString("0.00");
            }
        }

        void SetVoltage(IPacket packet)
        {
            if (tbVoltage.InvokeRequired)
                this.Invoke(new Action<IPacket>(this.SetVoltage), packet);
            else
            {
                XbeeSensorPacket sp = packet as XbeeSensorPacket;
                if (sp != null)
                {
                    tbVoltage.Text = (sp.AnalogSamples[7] * 1200 / 1024).ToString();
                }
            }
        }

        void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                pm.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(String.Concat("Open port manager failed.", Environment.NewLine, Environment.NewLine, ex.Message));
            }
        }

        void pm_DataDiscarded(string data)
        {
            AddDiscarded(data);
        }

        void pm_DataReceived(string data)
        {
            AddPacket(data);
        }

        private void AddPacket(string data)
        {
            if (listBoxPackets.InvokeRequired)
                this.Invoke(new Action<string>(this.AddPacket), data);
            else
            {
                listBoxPackets.Items.Add(data);
            }
        }

        private void AddDiscarded(string data)
        {
            if (listBoxDiscarded.InvokeRequired)
                this.Invoke(new Action<string>(this.AddDiscarded), data);
            else
            {
                listBoxDiscarded.Items.Add(data);
                listBoxDiscarded.SelectedIndex = -1;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _bw.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pm.Close();
        }
    }
}
