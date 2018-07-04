using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using FlexAdmin.Core;

namespace TopSpeedSQL
{
    public partial class Form1 : Form
    {
        TopSpeed _tps;
        int _queries = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private string GetAssemblyVersion()
        {
            return (FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location)).FileVersion;
        }

        private void button_SelectFolder_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dres = folderBrowserDialog1.ShowDialog();

                if (dres == System.Windows.Forms.DialogResult.OK)
                {
                    string path = folderBrowserDialog1.SelectedPath;

                    if (String.Compare(path, textBox_Location.Text, true) != 0)
                    {
                        textBox_Location.Text = path;

                        listView1.Items.Clear();
                        tabControl_Content.TabPages.Clear();
                        toolStripButton_Execute.Enabled = false;
                    }

                    if (ValidatePath(textBox_Location.Text))
                    {
                        Properties.Settings.Default.Path = path;
                        Properties.Settings.Default.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error initialising the database: Msg={0}", ex.Message));
            }
        }

        private bool ValidatePath(string path)
        {
            bool ok = false;

            _tps = null;

            toolStripButton_NewQuery.Enabled = false;
            toolStripButton_Execute.Enabled = false;

            if (path != String.Empty)
            {
                _tps = new TopSpeed(textBox_Location.Text);

                int x = ListTables(path);

                if (x > 0)
                {
                    toolStripButton_NewQuery.Enabled = true;
                }
            }

            return ok;
        }

        private void toolStripButton_NewQuery_Click(object sender, EventArgs e)
        {
            CreatePage();
        }

        private void CreatePage()
        {
            CreatePage(String.Empty);
        }

        private void CreatePage(string sql)
        {
            TabPage tp = new TabPage(String.Format("Query #{0}", ++_queries));
            SqlDataPage p = new SqlDataPage(_tps, sql);
            p.Dock = DockStyle.Fill;
            tp.Controls.Add(p);
            tabControl_Content.TabPages.Add(tp);
            tabControl_Content.SelectedTab = tp;

            toolStripButton_Execute.Enabled = true;
        }

        private void toolStripButton_Execute_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void Execute()
        {
            try
            {
                SqlDataPage p = tabControl_Content.SelectedTab.Controls[0] as SqlDataPage;
                p.Execute();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem i = listView1.SelectedItems[0];
                string sql = "select * from " + i.Text;
                CreatePage(sql);
                Execute();

            }
        }

        private void button_ToClarionDate_Click(object sender, EventArgs e)
        {
            int clariondt = _tps.ConvertToClarionDate(dateTimePicker_CalendarDate.Value);
            textBox_ClarionDate.Text = clariondt.ToString();
        }

        private void button_FromClarionDate_Click(object sender, EventArgs e)
        {
            int number;

            if (Int32.TryParse(textBox_ClarionDate.Text, out number))
            {
                DateTime dt = _tps.ConvertFromClarionDate(number);
                dateTimePicker_CalendarDate.Value = dt;
            }
        }

        private int ListTables(string path)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            int rows = 0;

            try
            {
                foreach (string file in Directory.GetFiles(path, "*.tps"))
                {
                    if (file.Contains(' '))
                    {
                        string newfile = file.Replace(' ', '_');
                        File.Move(Path.Combine(path, file), Path.Combine(path, newfile));
                    }
                }                        

                DataTable dt = _tps.GetTables();
                foreach (DataRow r in dt.Rows)
                {
                    ListViewItem i = new ListViewItem(r["TableName"].ToString());
                    listView1.Items.Add(i);
                }

                rows = dt.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error reading table list: Msg={0}", ex.Message), "ODBC Error");
            }

            listView1.EndUpdate();

            return rows;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel_VersionVal.Text = GetAssemblyVersion();
                listView1.Columns[0].Width = -2;
                textBox_Location.Text = Properties.Settings.Default.Path;
                ValidatePath(textBox_Location.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error during initialisation: Msg={0}", ex.Message), "Initialisation Error");
                Application.Exit();
            }
        }
    }
}
