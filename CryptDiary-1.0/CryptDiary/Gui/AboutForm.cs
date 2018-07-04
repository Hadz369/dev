using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Reflection;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptDiary.Gui
{
    public partial class AboutForm : Form
    {
        private ResourceManager messageManager = new ResourceManager("CryptDiary.Resources.Messages", typeof(AboutForm).Assembly);
        private Version AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public AboutForm()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(new DiarySettings().CultureInfo);
            InitializeComponent();

            // Try to show program version. If that fails, show assembly version

            try
            {
                // programm version
                Version programVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                versionLabel.Text = programVersion.Major.ToString() + "." + programVersion.Minor.ToString();
            }
            catch (InvalidDeploymentException)
            {
                // assembly version
                versionLabel.Text = AssemblyVersion.ToString() + " (assembly version)";
            }
        }

        private void btcAddressLabel_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((sender as Label).Text);
            MessageBox.Show("\"" + (sender as Label).Text + "\" is now in your clipboard.");
        }
    }
}