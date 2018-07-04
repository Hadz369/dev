using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using HS;
using HS.Network;
using HS.Network.WCF;
using Newtonsoft.Json;

namespace AjaxTest
{
    public partial class Default : System.Web.UI.Page
    {
        DuplexChannelFactory<IHSCBServiceContract> _cf;
        HSCallback _cb = null;
        InstanceContext _instanceContext = null;
        IHSCBServiceContract _proxy = null;
        Dictionary<string, string> _myList = new Dictionary<string, string>();
        System.Threading.Timer _timer;

        bool _tick = true;
        int _event = 0;
        Guid _guid = Guid.NewGuid();

        public Default()
        {
            _timer = new System.Threading.Timer(OnTimer);
            _timer.Change(500, 500);

            _cb = new HSCallback();
            _cb.CallbackEvent += _cb_CallbackEvent;

            _instanceContext = new InstanceContext(_cb);

            _cf = new DuplexChannelFactory<IHSCBServiceContract>(_cb, "DuplexClient");
            _cf.Open();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DoDataBinding();
            }
        }

        void OnTimer(object state)
        {
            lbxHelloWorld.Items.Add("Tick");
        }

        private void DoDataBinding()
        {
            lbxHelloWorld.DataSource = _myList;
            lbxHelloWorld.DataValueField = "Value";
            lbxHelloWorld.DataBind();
        }

        protected void _cb_CallbackEvent(string data)
        {
            try
            {   
                Packet<EnergyMeter> pkt = Newtonsoft.Json.JsonConvert.DeserializeObject<Packet<EnergyMeter>>(data);
                //_myList.Add((++_event).ToString(), data);
                lbxHelloWorld.Items.Add(data);
//                lblHelloWorld.Text = data;
            }
            catch (Exception ex)
            {
                Tracer.Error("Error processing callback", ex);
            }
        }

        protected void btnHelloWorld_Click(object sender, EventArgs e)
        {
            lblHelloWorld.Text = "Clicked at " + DateTime.Now.ToString();
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                _proxy = _cf.CreateChannel();
                _proxy.Register(_guid);
            }
            catch (Exception ex)
            {
                Tracer.Error("Error registering channel", ex);
                btnStop_Click(sender, e);
            }
        }

        protected void btnDeregister_Click(object sender, EventArgs e)
        {
            _proxy = _cf.CreateChannel();
            _proxy.Deregister(_guid);
        }

        protected void btnStop_Click(object sender, EventArgs e)
        {
            //((IClientChannel)_proxy).Close();
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            if (_cb == null)
            {
            }
        }
    }

    public delegate void CallbackMessageHandler(string data);

    public class HSCallback : IHSCallback
    {
        public event CallbackMessageHandler CallbackEvent;

        public void Callback(string data)
        {
            if (CallbackEvent != null)
                CallbackEvent(data);
        }
    }

    class MsgItem
    {
        public MsgItem(string msg)
        {
            Value = msg;
        }

        public string Value { get; private set; }
    }
}