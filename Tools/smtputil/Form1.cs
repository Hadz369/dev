using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace smtputil
{
    public partial class Form1 : Form
    {
        Thread _thread;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtServer.Text = Properties.Settings.Default.Server;
            txtPort.Text = Properties.Settings.Default.Port;
            txtUser.Text = Properties.Settings.Default.User;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtSender.Text = Properties.Settings.Default.Sender;
            chkUseSSL.Checked = Convert.ToBoolean(Properties.Settings.Default.UseSSL);
            foreach (string recipient in Properties.Settings.Default.Recipients)
                lstRecipients.Items.Add(recipient);

            txtSubject.Text = Properties.Settings.Default.Subject;
            txtMessage.Text = Properties.Settings.Default.Message;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(txtSender.Text);
                foreach (string r in lstRecipients.Items)
                    message.To.Add(new MailAddress(r));

                message.Subject = txtSubject.Text;
                message.Body = txtMessage.Text;

                _thread = new Thread(new ParameterizedThreadStart(SendMessage));
                _thread.Start(message);
                SetButtonState(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected Error: " + ex.Message + (ex.InnerException != null ? "\n\n" + ex.InnerException.Message : ""));
                SetButtonState(true);
            }
        }

        void SendMessage(object data)
        {
            try
            {
                SmtpClient client = new SmtpClient(txtServer.Text, Convert.ToInt32(txtPort.Text));
                client.Credentials = new NetworkCredential(txtUser.Text, txtPassword.Text);
                client.EnableSsl = chkUseSSL.Checked;
                client.Send(data as MailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Sending Mail: " + ex.Message + (ex.InnerException != null ? "\n\n" + ex.InnerException.Message : ""));
            }

            SetButtonState(true);
        }

        delegate void SetStateDelegate(bool state);

        void SetButtonState(bool enabled)
        {
            if (btnSend.InvokeRequired)
                btnSend.Invoke(new SetStateDelegate(SetButtonState), enabled);
            else
                btnSend.Enabled = enabled;

        }
    }
}
