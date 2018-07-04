using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StockControl.Common;

namespace StockControl
{
    public partial class MainForm : Form
    {
        Client c;

        public MainForm()
        {
            InitializeComponent();
            c = new Client();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text != "Stop")
            {
                c.Start();
                button1.Text = "Stop";
            }
            else
            {
                c.Stop();
                button1.Text = "Start";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("Starting");
            try
            {
                //listBox1.Items.Add("tcp: " + c.DataHandler.ReverseString(textBox1.Text));
                listBox1.Items.Add("web : " + c.WebHandler.ReverseString(textBox1.Text));
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.Message);
            }
        }
    }
}
