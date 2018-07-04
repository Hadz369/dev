﻿using System;
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
using System.Windows.Shapes;

namespace SampleProject
{
    /// <summary>
    /// Interaction logic for TabItemsWindow.xaml
    /// </summary>
    public partial class TabItemsWindow : Window
    {
        public TabItemsWindow()
        {
            InitializeComponent();
                         
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SampleView view = new SampleView();
            
            view.LabelText = "A new view #" + (this.tabControl.Items.Count).ToString();
            this.tabControl.Items.Add(view);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CancelCloseEventWindow newWindow = new CancelCloseEventWindow();
            newWindow.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window1 newWindow = new Window1();
            newWindow.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SimpleClosableTabWindow newWindow = new SimpleClosableTabWindow();
            newWindow.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NoTabCloseWindow newWindow = new NoTabCloseWindow();
            newWindow.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NoXamlWindow newWindow = new NoXamlWindow();
            newWindow.Show();
        }

        
    }
}
