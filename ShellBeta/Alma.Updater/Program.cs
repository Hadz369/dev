using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Alma.Updater
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show("No update control file parameter specified", "Missing Control File", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Window w = new Window();
                w.Width = 100;
                w.Height = 200;
                w.WindowStyle = WindowStyle.ToolWindow;

                Activity a = new Activity();
                a.HorizontalAlignment = HorizontalAlignment.Stretch;
                a.VerticalAlignment = VerticalAlignment.Stretch;

                w.Content = a;
                w.ShowDialog();
            }
        }
    }
}
