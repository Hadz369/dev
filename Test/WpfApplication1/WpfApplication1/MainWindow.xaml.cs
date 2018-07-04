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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<TestItem> items = new List<TestItem>() {
                new TestItem() { IsChecked = true, IsNotChecked = false, Text="Hello World. I am having a terrible day.", Link="My Link 1" },
                new TestItem() { IsChecked = false, IsNotChecked = true, Text="Hello World. I hope your day is nice.", Link="My Link 1" },
                new TestItem() { IsChecked = true, IsNotChecked = false, Text="Hello World. Yours is nice? Shut up!!", Link="My Link 1" },
                new TestItem() { IsChecked = false, IsNotChecked = true, Text="Hello World. Ok, sorry for my outburst. I don't think I will yell at you again.", Link="My Link 1" },
                new TestItem() { IsChecked = true, IsNotChecked = false, Text="Hello World. Time to go home almost. Just have to get the installer to build properly and then we should be sweet.", Link="My Link 1" } };

            dataGrid1.ItemsSource = items;
            
        }

        class TestItem
        {
            public bool IsChecked { get; set; }
            public bool IsNotChecked { get; set; }
            public string Text { get; set; }
            public string Link { get; set; }
        }
    }
}
