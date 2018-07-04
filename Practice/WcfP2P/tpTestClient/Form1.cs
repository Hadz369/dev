using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace IG.ThirdParty
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class Form1 : Form, IQuickReturnTraderChat
    {
        IQuickReturnTraderChat channel;
        ServiceHost host = null;
        ChannelFactory<IQuickReturnTraderChat> cf = null;
        string userId = "";
        
        public Form1()
        {
            InitializeComponent();
        }

        private void StartService()
        {
            host = new ServiceHost(this);
 
            NetPeerTcpBinding binding = new NetPeerTcpBinding();
            binding.Name = "BindingUnsecure";
            binding.Security.Mode = SecurityMode.None;
            binding.Resolver.Mode = System.ServiceModel.PeerResolvers.PeerResolverMode.Pnrp;

            host.AddServiceEndpoint(
                typeof(IQuickReturnTraderChat),
                binding,
                "net.p2p://QuickReturnTraderChat");

            host.Open();

            cf = new ChannelFactory<IQuickReturnTraderChat>("QuickTraderChatEndpoint");
            channel = cf.CreateChannel();
            channel.Say("Admin", "*** New User " + userId + " Joined ****" + Environment.NewLine);
        }


        void IQuickReturnTraderChat.Say(string user, string message)
        {
            throw new NotImplementedException();
        }

        private void btnSignOn_Click(object sender, EventArgs e)
        {

        }
    }
}
