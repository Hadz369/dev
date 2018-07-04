using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;

namespace StockControl.Common
{
    [ServiceContract]
    public interface IWebHandler
    {
        [OperationContract]
        string ReverseString(string value);
    }

    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IMessageHandlerCallback))]
    public interface IMessageHandler
    {
        [OperationContract]
        void Subscribe();
        [OperationContract]
        void Unsubscribe();
        [OperationContract]
        void Stop();
    }

    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IClientHandlerCallback))]
    public interface IClientHandler
    {
        [OperationContract]
        void Subscribe();
        [OperationContract]
        void Unsubscribe();
        [OperationContract]
        DataTable GetTypes();
        [OperationContract]
        void Stop();
        [OperationContract]
        int DoStuff();
    }
}
