using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IG.ThirdParty
{
    public partial class Form1 : Form
    {
        tpClient _client;

        int _session;

        public Form1()
        {
            InitializeComponent();
            _client = new tpClient("net.tcp://localhost:8000");
        }

        private void btnSiteDetails_Click(object sender, EventArgs e)
        {
            string name = _client.GetSiteDetails();
            listBox1.Items.Add(name);
        }

        private void btnSignOn_Click(object sender, EventArgs e)
        {
            _session = _client.SignOn();
            listBox1.Items.Add(_session.ToString());
        }

        private void btnAllMembers_Click(object sender, EventArgs e)
        {

        }
    }
}
