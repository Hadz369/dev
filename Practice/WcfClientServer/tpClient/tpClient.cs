using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace IG.ThirdParty
{
    public class tpClient
    {
        ChannelFactory<IService> _cfact;
        int _session;
        string _endpoint;
        IService _svc;

        public tpClient(string endpointAddress)
        {
            _endpoint = endpointAddress;
            _cfact = new ChannelFactory<IService>(new NetTcpBinding(), endpointAddress);
            Connect();
        }

        private void Connect()
        {
            Disconnect();            
            _svc = _cfact.CreateChannel();
        }

        public void Disconnect()
        {
            if (_svc != null)
            {
                ICommunicationObject c = (ICommunicationObject)_svc;
                c.Close();
            }
        }

        public int SignOn()
        {
            TcpResponse rc = _svc.SignOn();
            if (rc.ResponseCode == 0)
            {
                _session = ((SessionInfo)rc.Data).SessionId;
            }

            return _session;
        }

        public string GetSiteDetails()
        {
            TcpResponse rc = _svc.GetSiteName(new TcpRequest(_session));
            string name = rc.Data.ToString();

            return name;
        }
    }
}
