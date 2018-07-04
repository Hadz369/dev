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
using System.Threading.Tasks;
using M1;

namespace TabControlTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ARCRESTClient _client;

        MainViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            mainViewModel = new MainViewModel();
            TabItemViewModel vm;

            vm = new TabItemViewModel("ReportViewer Template", TabContentType.ReportViewer);
            vm.CloseEvent += vm_CloseEvent;
            mainViewModel.Items.Add(vm);
            
            vm = new TabItemViewModel("Crystal Template", TabContentType.Crystal);
            vm.CloseEvent += vm_CloseEvent;
            mainViewModel.Items.Add(vm);
            
            vm = new TabItemViewModel("DataGrid Template", TabContentType.DataGrid);
            vm.CloseEvent += vm_CloseEvent;
            mainViewModel.Items.Add(vm);

            this.DataContext = mainViewModel;

            mainViewModel.SelectedItem = mainViewModel.Items[0];
        }

        void vm_CloseEvent(object sender, EventArgs e)
        {
            var item = sender as TabItemViewModel;
            if (item != null)
                mainViewModel.Items.Remove(item);
        }
        
        private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var tabItem = e.Source as TabItem;

            if (tabItem == null)
                return;

            if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(tabItem, tabItem, DragDropEffects.All);
            }
        }


        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            var tabItemSource = e.Data.GetData(typeof(TabItem)) as TabItem;
            var tabItemTarget = e.Source as TabItem;

            TabItemViewModel svm, tvm;

            svm = tabItemSource.Content as TabItemViewModel;
            tvm = tabItemTarget.Content as TabItemViewModel;

            int sourceIndex = mainViewModel.Items.IndexOf(svm);
            int targetIndex = mainViewModel.Items.IndexOf(tvm);

            //mainViewModel.Items.Add(vm);

            //var tabItemSource = e.Data.GetData(typeof(TabItem)) as TabItem;

            //if (!tabItemTarget.Equals(tabItemSource))
            //{
            //    var tabControl = tabControl77; // tabItemTarget.Parent as TabControl;
                
            //    int sourceIndex = tabControl.Items.IndexOf(tabItemSource);
            //    int targetIndex = tabControl.Items.IndexOf(tabItemTarget);

            //    mainViewModel.Reports.RemoveAt(sourceIndex);
            //    //tabControl.Items.Remove(tabItemSource);
            //    tabControl.Items.Insert(targetIndex, tabItemSource);

            //    //tabControl.Items.Remove(tabItemTarget);
            //    tabControl.Items.Insert(sourceIndex, tabItemTarget);
            //}

            if (svm != null)
            {
                if (mainViewModel.Items.Contains(svm))
                    mainViewModel.Items.Remove(svm);
            }

            mainViewModel.Items.Insert(targetIndex, svm);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowManager.ApplicationClosing = true;

            foreach (Window window in WindowManager.Windows)
            {
                window.Close();
            }
        }
    }
}
