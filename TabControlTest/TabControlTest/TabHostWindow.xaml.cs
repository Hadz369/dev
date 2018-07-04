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
using Dragablz;

namespace TabControlTest
{
    /// <summary>
    /// Interaction logic for TabHostWindow.xaml
    /// </summary>
    public partial class TabHostWindow : DragablzWindow
    {
        public TabHostWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WindowManager.Remove(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowManager.Add(this);
        }
    }
}