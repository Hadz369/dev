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
using System.Data;
using System.Data.SqlClient;
using WpfSearchControls;

namespace SearchTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Business _business = Business.Instance;
        Address _address;

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Test";

            _business.Init(Properties.Settings.Default.ConString);
            _address = new Address();
            _address.PropertyChanged += Address_PropertyChanged;
            addressEditor.Init(_address);

            this.Loaded += MainWindow_Loaded;
        }

        void Address_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_address.IsValid)
                btnAccept.IsEnabled = true;
            else
                btnAccept.IsEnabled = false;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Accepted");
        }
    }
}
