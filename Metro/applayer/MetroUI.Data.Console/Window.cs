using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml.Serialization;
using System.IO;
using System.ServiceModel;
using Metro.Service;

namespace Metro
{
    public partial class Window : Form, IDisposable
    {
        ChannelBroker<ISystemServiceContract> _sysbroker = ChannelBroker<ISystemServiceContract>.Instance;
        ChannelBroker<IFlexiNetServiceContract> _flxbroker = ChannelBroker<IFlexiNetServiceContract>.Instance;

        ChannelProxy<IFlexiNetServiceContract> _flexi;

        int _counter = 0;
        Object _locker = new Object();

        public Window()
        {
            InitializeComponent();

            _flexi = _flxbroker.GetProxy("FlexiNet");
        }

        int IncrementCounter()
        {
            lock (_locker)
            {
                return ++_counter;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            Response r = _flexi.Channel.ProcessRequest(new Request("key", "Machine", "GetMachineList"));

            textBox1.Text = r.Data.ToString();

            using (ChannelProxy<IMetroServiceContract> cprox = _broker.GetProxy("Cache"))
            {
                try
                {
                    Request req = new Request("key", "Code", "RefreshCodes");

                    Response res = cprox.Channel.ProcessRequest(req);

                    FaultData fd = res.Data as FaultData;

                    if (fd != null)
                    {
                        textBox1.Text = fd.Message;
                    }
                    else
                    {
                        req.Header.Action = "GetCodes";
                        req.Header.Handler = "Code";

                        req.PropertyBag.Clear();
                        req.PropertyBag.Add(new Property("TypeDefn", "METERTYPE"));

                        res = cprox.Channel.ProcessRequest(req);

                        if (res.Data != null)
                        {
                            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(res.Data.GetType());
                            System.IO.MemoryStream ms = new System.IO.MemoryStream();
                            x.Serialize(ms, res.Data);

                            ms.Position = 0;
                            System.IO.StreamReader sr = new System.IO.StreamReader(ms);
                            string myStr = sr.ReadToEnd();

                            textBox1.Text += "\r\n-----------------------------------------------\r\n";
                            textBox1.Text += myStr;
                        }
                    }
                }
                catch (FaultException ex)
                {
                    Tracer.Error("Error doing things of importance: FaultCode=" + ex.Code, ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (ex.InnerException != null) Console.WriteLine(ex.InnerException);

                    Tracer.Error("Shit", ex);
                }
                finally
                {
                    Tracer.Error("Why did I get here?");
                }
            }
            */
        }

        private void btnAsyncTest_Click(object sender, EventArgs e)
        {
            using (ChannelProxy<IFlexiNetServiceContract> flexi = _flxbroker.GetProxy("FlexiNet"))
            {

                Request request = new Request(tbToken.Text, "Machine", "GetMachineList");
                
                IAsyncResult result = flexi.Channel.BeginRequestAsync(request, null, null);
                Console.WriteLine("Request started");

                while (!result.IsCompleted)
                {
                    textBox1.Text += ".";
                }

                Response response = flexi.Channel.EndRequestAsync(result);

                UpdateText(response.Data.ToString());

                Console.WriteLine("Done");
            }
        }

        private void btnAsyncCallback_Click(object sender, EventArgs e)
        {
            Request request = new Request(tbToken.Text, "Machine", "GetMachineList");
            Response response = new Response(request.Header);

            _flexi.Channel.BeginRequestAsync(request, new AsyncCallback(DisplayCallbackData), null);

            Console.WriteLine("Request started");
        }

        private void DisplayCallbackData(IAsyncResult result)
        {
            Response response = _flexi.Channel.EndRequestAsync(result);
            UpdateText(response.Data.ToString());
        }

        delegate void UpdateTextDelegate(string message);

        private void UpdateText(string message)
        {
            if (textBox1.InvokeRequired)
            {
                BeginInvoke(new UpdateTextDelegate(UpdateText), message);
            }
            else
            {
                textBox1.Text = IncrementCounter().ToString() + "\r\n" + message;
            }
        }

        void IDisposable.Dispose()
        {
            _flexi.Dispose();
        }

        private void btnSynchronous_Click(object sender, EventArgs e)
        {
            Request request = null;
            Response response = null;

            if (rbGetMemberList.Checked)
            {
                request = new Request(tbToken.Text, "Member", "GetMemberList");
                if (tbDateFrom.Text != String.Empty)
                    request.PropertyBag.Add(new Property("DateFrom", DateTime.Parse(tbDateFrom.Text)));

                using (ChannelProxy<IFlexiNetServiceContract> p = _flxbroker.GetProxy("FlexiNet"))
                    response = p.Channel.ProcessRequest(request);
            }
            else if (rbGetMemberTiers.Checked)
            {
                request = new Request(tbToken.Text, "Member", "GetMemberTiers");

                using (ChannelProxy<IFlexiNetServiceContract> p = _flxbroker.GetProxy("FlexiNet"))
                    response = p.Channel.ProcessRequest(request);
            }
            else if (rbGetMachineList.Checked)
            {
                request = new Request(tbToken.Text, "Machine", "GetMachineDetails");

                using (ChannelProxy<ISystemServiceContract> p = _sysbroker.GetProxy("System"))
                    response = p.Channel.ProcessRequest(request);
            }
            else if (radioButton1.Checked)
            {
                request = new Request(tbToken.Text, "ThirdParty", "GetMemberInfo");
                request.PropertyBag.Add(new Property("CardNumber", tbCardNumber.Text));

                try
                {
                    using (ChannelProxy<ISystemServiceContract> p = _sysbroker.GetProxy("System"))
                        response = p.Channel.ProcessRequest(request);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (response != null && response.Data != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(response.Data.GetType());
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, response.Data);

                UpdateText(textWriter.ToString());
            }
        }

        private void btnSignOn_Click(object sender, EventArgs e)
        {
            Request request   = new Request("", "Session", "SignOn");
            request.PropertyBag.Add(new Property("Vendor", tbVendor.Text));
            request.PropertyBag.Add(new Property("Device", Int32.Parse(tbDevice.Text)));

            Response response = null;

            using (ChannelProxy<ISystemServiceContract> p = _sysbroker.GetProxy("System"))
                response = p.Channel.ProcessRequest(request);

            if (response != null)
            {
                if (!response.IsFault)
                {
                    Session s = response.Data as Session;

                    if (s != null)
                    {
                        tbToken.Text = s.SessionKey;
                        tbToken.Tag = s;
                    }
                }

                else
                {
                    FaultData fd = response.Data as FaultData;
                    UpdateText(String.Format("Fault: Code={0}, Message={1}", fd.Code, fd.Message));
                }
            }
        }

        private void btnSignOff_Click(object sender, EventArgs e)
        {
            Request request = new Request(tbToken.Text, "Session", "SignOff");

            Response response = null;

            using (ChannelProxy<ISystemServiceContract> p = _sysbroker.GetProxy("System"))
                response = p.Channel.ProcessRequest(request);

            if (response != null)
            {
                tbToken.Text = "";
                tbToken.Tag = null;

                UpdateText("Session closed");
            }
        }
    }
}
