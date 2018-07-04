using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CryptDiary.Gui
{

    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            // localisation of GUI
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(new DiarySettings().CultureInfo);
            InitializeComponent();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
            // Letzten Teil des WorkingDirectories in das Label schreiben
            string workDirectory = new DiarySettings().WorkDirectory;
            if (Directory.Exists(workDirectory))
            {
                string[] DirectoryParts = workDirectory.Split('\\');
                string workDirectoryLastDirectory = DirectoryParts[DirectoryParts.Length - 1];
                workDirectoryLabel.Text = workDirectoryLastDirectory;
            }
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // enter means OK, Escape means Cancel
            if (e.KeyCode == Keys.Return)
            {
                DialogResult = DialogResult.OK;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
