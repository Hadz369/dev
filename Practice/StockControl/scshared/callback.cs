using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;

namespace StockControl.Common
{
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void MyCallbackFunction(string callbackValue);
    }

    public interface IMessageHandlerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnCallback(string message);
    }

    public interface IClientHandlerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnCallback(string message);
    }

    public class Callback : ICallback
    {
        public void MyCallbackFunction(string callbackValue)
        {
            Console.WriteLine("Callback Received: {0}", callbackValue);
        }
    }

    public class MessageHandlerCallback : IMessageHandlerCallback
    {
        public void OnCallback(string message)
        {
            Console.WriteLine("Message Event");
            //throw new NotImplementedException();
        }
    }

    public class ClientHandlerCallback : IClientHandlerCallback
    {
        public void OnCallback(string message)
        {
            Console.WriteLine("Client Event");
        }
    }
}
