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
    public partial class ChangePasswordForm : Form
    {
        // localisation of messages
        private ResourceManager messageManager = new ResourceManager("CryptDiary.Resources.Messages", typeof(ChangePasswordForm).Assembly);

        public ChangePasswordForm()
        {
            // localisation of GUI
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(new DiarySettings().CultureInfo);
            InitializeComponent();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
            // write last part of workDirectory to the label
            string workDirectory = new DiarySettings().WorkDirectory;
            if (Directory.Exists(workDirectory))
            {
                string[] directoryParts = workDirectory.Split('\\');
                string workDirectoryLastDirectory = directoryParts[directoryParts.Length - 1];
                workDirectoryLabel.Text = workDirectoryLastDirectory;
            }
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter confirms dialog, Escape cancels
            if (e.KeyCode == Keys.Return)
            {
                okButton_Click(this, null);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {   
            // catch all possible wrong user inputs
            if (oldPasswordTextBox.Text == "")
            {
                MessageBox.Show(messageManager.GetString("PromptEnterOldPassword"));
            }
            else if (newPasswordTextBox1.Text == "")
            {
                MessageBox.Show(messageManager.GetString("PromptEnterNewPassword"));
            }
            else if (newPasswordTextBox1.Text != "" && newPasswordTextBox2.Text == "")
            {
                MessageBox.Show(messageManager.GetString("PromptConfirmNewPassword"));
            }
            else if (newPasswordTextBox1.Text != newPasswordTextBox2.Text)
            {
                MessageBox.Show(messageManager.GetString("InfoPasswordsNotEqual"));
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
