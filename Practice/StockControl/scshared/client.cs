using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;

namespace StockControl.Common
{
    public class Client
    {
        bool _stopping = false;

        IMessageHandler _tcpMessageProxy; 
        IClientHandler _tcpClientProxy;
        IWebHandler _webClientProxy;

        DuplexChannelFactory<IMessageHandler> _messageFactory;
        DuplexChannelFactory<IClientHandler> _clientFactory;
        ChannelFactory<IWebHandler> _webFactory;

        public Client()
        {
            ClientHandlerCallback clientCallback = new ClientHandlerCallback();
            MessageHandlerCallback messageCallback = new MessageHandlerCallback();

            _messageFactory = new DuplexChannelFactory<IMessageHandler>(
                messageCallback,
                new NetTcpBinding(SecurityMode.Transport),
                new EndpointAddress("net.tcp://localhost:8000/MessageHandler"));
            _messageFactory.Faulted += new EventHandler(_messageFactory_Faulted);

            _clientFactory = new DuplexChannelFactory<IClientHandler>(
                clientCallback,
                new NetTcpBinding(SecurityMode.Transport),
                new EndpointAddress(
                  "net.tcp://localhost:8001/ClientHandler"));
            _clientFactory.Faulted += new EventHandler(_clientFactory_Faulted);

            _webFactory = new ChannelFactory<IWebHandler>(
                new BasicHttpBinding(),
                new EndpointAddress("http://localhost:8002/WebHandler"));
            _webFactory.Faulted += new EventHandler(_webFactory_Faulted);
        }

        void _webFactory_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Web factory faulted");
        }

        void _clientFactory_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Client factory faulted");
        }

        void _messageFactory_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Message factory faulted");
        }

        public IMessageHandler MessageHandler { get { return _tcpMessageProxy; } set { _tcpMessageProxy = value; } }
        public IClientHandler ClientHandler { get { return _tcpClientProxy; } set { _tcpClientProxy = value; } }
        public IWebHandler WebHandler { get { return _webClientProxy; } set { _webClientProxy = value; } }

        public void StartMessageHandler()
        {
            _tcpMessageProxy = _messageFactory.CreateChannel();
        }

        public void StartClientHandler()
        {
            _tcpClientProxy = _clientFactory.CreateChannel();
        }

        public void StartWebHandler()
        {
            _webClientProxy = _webFactory.CreateChannel();
        }

        public void StopMessageHandler()
        {
            _tcpMessageProxy.Unsubscribe();
            _messageFactory.Close();
        }

        public void StopClientHandler()
        {
            _tcpClientProxy.Unsubscribe();
            _clientFactory.Close();
        }

        public void StopWebHandler()
        {
            _webFactory.Close();
        }

    }
}
