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
using System.Diagnostics;
using Hadz369.Framework;

namespace FrameworkTestHarness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            H369TextWriterTraceListener _twListener = new H369TextWriterTraceListener(
                new H369TraceFileOptions("C:\\Temp", "FrameworkTestHarness")) { TraceLevel = (TraceLevel)4 };

            H369Trace.Add(_twListener);

            H369MessageEventListener _msgListener = new H369MessageEventListener() { TraceLevel = (TraceLevel)4 };
            _msgListener.MessageEvent += _msgListener_MessageEvent;
            H369Trace.Add(_msgListener);
        }

        void _msgListener_MessageEvent(MessageEventData eventData)
        {
            lbLog.Items.Add(eventData.Message);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoStuff();
            }
            catch(Exception ex)
            {
                H369Trace.WriteLine(new H369TraceData("Error doing stuff", ex));
            }
        }

        void DoStuff()
        {
            Dictionary<string, object> parms = new Dictionary<string,object>();
            parms.Add("Parm1", 123);
            parms.Add("Parm2", new DateTime(2013,2,3));

            using (MethodLogger mw = new MethodLogger("DoStuff", parms ))
            {
                System.Threading.Thread.Sleep(1000);

                throw new Exception("Test");
            }
        }
    }
}
