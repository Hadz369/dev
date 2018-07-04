using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArduinoUDP
{
    public partial class formDataViewer : Form
    {
        public formDataViewer()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            dataGridView1.DataSource = null;
        }

        public void LoadGrid(DataTable DT)
        {
            if (DT == null)
            {
                throw new Exception("An invalid data table was provided for the LoadGrid method");
            }
            else
            {
                this.SuspendLayout();

                Clear();
                dataGridView1.DataSource = DT;

                this.ResumeLayout();
            }
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            Clear();
            this.Close();
        }
    }
}
