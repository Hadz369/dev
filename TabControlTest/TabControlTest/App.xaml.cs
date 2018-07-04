using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using System.Threading.Tasks;
using M1;

namespace TabControlTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ARCRESTClient.Instance.Initialise(new Uri("http://localhost:28108/eps/execute"), 102, null);

            int rc = ARCRESTClient.Instance.Connect();

            if (rc == 0)
            {
                ARCRESTClient.Instance.StartMonitor();
                UserSession session = ARCRESTClient.Instance.Login("eps", "ebet");
                if (session == null)
                {
                    MessageBox.Show("Login failed for user 'eps'");
                }
            }
            else
            {
                MessageBox.Show(String.Concat("Error connection to the ARC gateway; RC=", rc));
            }

            MainWindow w = new MainWindow();
            w.Show();
        }
    }
}
