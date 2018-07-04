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
using System.ComponentModel;

using HS;
using HS.Network;
using HS.Network.WCF;
using Newtonsoft.Json;

namespace hsclient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DuplexChannelFactory<IHSCBServiceContract> _cf;
        HSCallback _cb = null;
        InstanceContext _instanceContext = null;
        IHSCBServiceContract _proxy = null;
        MyViewModel _vm = new MyViewModel();
        Guid _guid = Guid.NewGuid();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _vm;
        }

        void _cb_CallbackEvent(string data)
        {
            try
            {
                Packet<EnergyMeter> pkt = Newtonsoft.Json.JsonConvert.DeserializeObject<Packet<EnergyMeter>>(data);

                lbMessages.Items.Add(pkt.dat.Meter.ToString());
            }
            catch (Exception ex)
            {
                Tracer.Error("Error during callback processing", ex);
            }
        }

       
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _proxy = _cf.CreateChannel();
                _proxy.Register(_guid);
            }
            catch (Exception ex)
            {
                Tracer.Error("Error registering channel", ex);
                btnStop_Click(sender, e);
            }
        }

        private void btnUnregister_Click(object sender, RoutedEventArgs e)
        {
            _proxy = _cf.CreateChannel();
            _proxy.Deregister(_guid);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            ((IClientChannel)_proxy).Close();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_cb == null)
            {
                _cb = new HSCallback();
                _cb.CallbackEvent += _cb_CallbackEvent;

                _instanceContext = new InstanceContext(_cb);

                _cf = new DuplexChannelFactory<IHSCBServiceContract>(_cb, "DuplexClient");

                _cf.Open();
            }
        }
    }

    public delegate void CallbackMessageHandler(string data);

    public class HSCallback : IHSCallback
    {
        public event CallbackMessageHandler CallbackEvent;

        public void Callback(string data)
        {
            if (CallbackEvent != null)
                CallbackEvent(data);
        }
    }

    public class MyViewModel :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        int _value = 0;

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
