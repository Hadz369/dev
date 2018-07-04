using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlexAdmin.Core;

namespace TopSpeedSQL
{
    public partial class SqlDataPage : UserControl
    {
        TopSpeed _tps;

        public SqlDataPage(TopSpeed tps)
        {
            Initialise(tps, String.Empty);
        }

        public SqlDataPage(TopSpeed tps, string sql)
        {
            Initialise(tps, sql);
        }

        private void Initialise(TopSpeed tps, string sql)
        {
            InitializeComponent();
            _tps = tps;

            if (sql != String.Empty) richTextBox_SQL.Text = sql;
        }

        public override string Text { get { return richTextBox_SQL.Text; } }

        public void Execute()
        {
            richTextBox_Messages.Clear();
            dataGridView1.DataSource = null;

            if (richTextBox_SQL.Text.Trim() != String.Empty)
            {
                object o = _tps.Execute(richTextBox_SQL.Text);

                if (o.GetType() == typeof(DataTable))
                {
                    DataTable dt = o as DataTable;
                    AddMessage(String.Format("Execution complete. {0} rows returned", dt.Rows.Count));
                    SetDataSource(o as DataTable);
                }
                else if (o.GetType() == typeof(int))
                {
                    int rc = (int)o;
                    AddMessage(String.Format("Execution complete. {0} rows affected", rc));
                }
            }
        }

        private void AddMessage(string message)
        {
            if (richTextBox_Messages.Text.Trim() != String.Empty)
                richTextBox_Messages.Text += String.Format("\n{0}", message);
            else
                richTextBox_Messages.Text = message;

            tabControl_SqlResults.SelectedTab = tabPage_Messages;
        }

        private void SetDataSource(DataTable data)
        {
            dataGridView1.DataSource = data;
            
            if (data.Rows.Count > 0)
                tabControl_SqlResults.SelectedTab = tabPage_Results;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    // Add the selection to the clipboard.
                    Clipboard.SetDataObject(dataGridView1.GetClipboardContent());
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    AddMessage("The Clipboard could not be accessed. Please try again.");
                }
            }
        }
    }
}
