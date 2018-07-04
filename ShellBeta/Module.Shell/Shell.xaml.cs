using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.IO;
using Alma.Core;
using Alma.Controls;

namespace Alma.Module
{
    /// <summary>
    /// The shell is a window used to host modules and provide the navigation and communication
    /// between these modules. A WCF service is hosted within the shell called the ModuleManager
    /// which coordinates messages between plugins. When a module is activated it should then
    /// register itself with the module manager to participate in messaging. Inheritance of the
    /// ShellBase and ModuleBase classes performs this automatically.
    /// </summary>
    public partial class Shell : ShellBase
    {
        public Shell() : base()
        {
            InitializeComponent();

            Tracer.WriteLine(new TracerData("Perfoming shell initialisation"));

            Tracer.WriteLine(new TracerData("Mapping gesture handlers"));

            GestureHandler gh = new GestureHandler();
            gh.AddBinding("CQ", () => tbxQuickSearch.Focus());
            gh.AddBinding("CH", () => ToggleHomePage());
            this.DataContext = gh;

            base.ModuleManager.LoadModules();
            foreach (IModule m in base.ModuleManager.Modules)
            {
            }
        }

        public override string ModuleDefn
        {
            get {  return "MODERNSHELL"; }
        }

        void _mm_MessageEvent(string message)
        {
            lbLogView.Items.Add(message);
        }

        /// <summary>
        /// TitleBar_MouseDown - Drag if single-click, resize if double-click
        /// </summary>
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    this.DragMove();
                }
            }
        }

        /// <summary>
        /// CloseButton_Clicked
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// MaximizedButton_Clicked
        /// </summary>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        /// <summary>
        /// Minimized Button_Clicked
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Adjusts the WindowSize to correct parameters when Maximize button is clicked
        /// </summary>
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        /// <summary>
        /// Shuts down the module manager when the applciation is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //_mm.Stop();
            //_mm.Dispose();
        }

        private void btnQuickSearch_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        private void tbxQuickSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            btnQuickSearch.IsDefault = true;
        }

        private void tbxQuickSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            btnQuickSearch.IsDefault = false;
        }

        private void ToggleHomePage()
        {
            MessageBox.Show("Home Page", "Bla Bla Bla");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test", "This is a test of the emergency lock mechanism");
        }
    }

    /// <summary>
    /// The gesture handler is used to perform an action when a key combination is 
    /// pressed. One or more actions can be added to the handler using the AddHandler method by
    /// providing a string key and an Action delegate.
    /// </summary>
    public class GestureHandler
    {
        public GestureHandler()
        {
            KeyBindingCommand = new KeyBindingCommand();
        }

        public KeyBindingCommand KeyBindingCommand { get; private set; }

        public void AddBinding(string key, Action action)
        {
            KeyBindingCommand.AddBinding(key, action);
        }
    }

    public class KeyBindingCommand : ICommand
    {
        Dictionary<string, Action> _bindings = new Dictionary<string, Action>();

        public void AddBinding(string key, Action action)
        {
            _bindings.Add(key, action);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            string parm = parameter.ToString();
            if (_bindings.ContainsKey(parm))
            {
                Action action = _bindings[parm];
                action();
            }
        }
    }
}
