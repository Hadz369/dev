using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Alma.Shared;

namespace Alma
{
    class Program
    {
        static string _exePath;

        [STAThread]
        static void Main(string[] args)
        {
            _exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Show the login dialog window first
            Login w = new Login();
            w.WindowStyle = System.Windows.WindowStyle.None;
            w.AllowsTransparency = false;
            w.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            bool? rc = w.ShowDialog();

            // If the user has successfully logged in then continue
            if (rc == true)
            {
                // todo: 
                // The login window should return the following:
                // > List of user and computer rights
                // > Availability of updates based on last update check time
                // > Service endpoints
                // > User messages

                // todo: If updates are available they should be copied here before loading any assemblies

                ShellBase shell = LoadShell();
                if (shell != null)
                {
                    try
                    {
                        ImageBrush myBrush = new ImageBrush();
                        myBrush.ImageSource = new BitmapImage(new Uri(String.Concat("pack://application:,,,", Properties.Settings.Default.WaterMarkImage), UriKind.Absolute));
                        shell.Background = myBrush;
                    }
                    catch(Exception ex)
                    {
                        // Log a message here
                    }

                    shell.ShowDialog();
                }
                else
                {
                    MessageBox.Show("The configured application shell is invalid. Contact software support.", 
                        "Error Loading Shell", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        /// <summary>
        /// Load the shell from the location specified in the app.config file.
        /// </summary>
        /// <returns></returns>
        public static ShellBase LoadShell()
        {
            string file = Path.Combine(_exePath, Properties.Settings.Default.ShellFileName);
            ShellBase sb = null;

            if (File.Exists(file))
            {
                try
                {
                    // Load the assembly file
                    Assembly shell = Assembly.LoadFile(file);

                    // Loop through the types in the loaded file
                    Type[] _types = shell.GetTypes();

                    foreach (Type _type in _types)
                    {
                        // If the type inherits from the ShellBase then create the instance
                        if (_type.BaseType == typeof(ShellBase))
                        {
                            sb = (ShellBase)Activator.CreateInstance(_type, new object[] { });
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error activating shell.\n\nFile={0}\n" +
                        "Exception Type={2}\nMsg={3}\n\nThe plugin was not loaded.",
                        file, ex.GetType(), ex.Message));
                }
            }

            return sb;
        }

        public static List<object> GetModules(string FileName)
        {
            List<object> _plugins = new List<object>();
            Assembly _assembly = Assembly.LoadFile(FileName);

            string _typeName = ""; // used for exception messages
            Type[] _types = _assembly.GetTypes();

            try
            {
                foreach (Type _type in _types)
                {
                    _typeName = _type.ToString();

                    if (_type.GetInterface("IPlugin") != null)
                    {
                        IModule _plugin = (IModule)Activator.CreateInstance(_type, new object[] { });
                        _plugins.Add(_plugin);
                    }
                }
            }
            catch (MissingMethodException ex)
            {
                MessageBox.Show(String.Format("File {0} is missing a required interface method " +
                    "in plugin {1}.\n\nMsg={2}\n\nThe plugin was not loaded.", FileName, _typeName, ex.Message));
            }
            catch (TargetInvocationException ex)
            {
                MessageBox.Show(String.Format("A TargetInvocationException error occured while activating a plugin.\nFile={0}\nPlugin Type={1}\n" +
                    "Exception Type={2}\nMsg={3}\n\nThe plugin was not loaded.",
                    FileName, _typeName, ex.InnerException.GetType(), ex.InnerException.Message));
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error activating plugin.\n\nFile={0}\nPlugin Type={1}\n" +
                    "Exception Type={2}\nMsg={3}\n\nThe plugin was not loaded.",
                    FileName, _typeName, ex.GetType(), ex.Message));
            }

            return _plugins;
        }
    }
}
