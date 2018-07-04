using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.Threading;

namespace RestServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebServiceHost _host;
        ServiceEndpoint _ep;
        Thread _thread;
        volatile bool _stop = false;

        public MainWindow()
        {
            InitializeComponent();

            _host = new WebServiceHost(typeof(Service), new Uri("http://localhost:8000"));
            _ep = _host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _thread = new Thread(Start);
            _thread.Start();

            while (!_thread.IsAlive) { }
        }

        private void Start()
        {
            _stop = false;

            _host.Open();

            while (!_stop) { }
        }

        private void Stop()
        {
            _stop = true;
            _thread.Join();
        }
    }
}
