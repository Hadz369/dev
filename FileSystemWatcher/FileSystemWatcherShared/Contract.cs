using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Reflection;

namespace FSW
{
    [ServiceKnownType("GetKnownTypes", typeof(FSWServiceKnownTypeHelper))]
    [ServiceContract()]
    public interface IFSWServiceCallback
    {
        [OperationContract]
        void Callback(Packet packet);
    }

    static class FSWServiceKnownTypeHelper
    {
        static List<Type> _knownTypes = new List<Type>();

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            // Only load the list the first time
            if (_knownTypes.Count == 0)
            {
                _knownTypes.Add(typeof(FSW.Packet));
                _knownTypes.Add(typeof(FSW.Body));
                _knownTypes.Add(typeof(FSW.FaultData));
                _knownTypes.Add(typeof(FSW.ErrorMessage));
                _knownTypes.Add(typeof(System.Data.DataTable));
                _knownTypes.Add(typeof(System.Data.DataSet));
                _knownTypes.Add(typeof(System.DBNull));
                _knownTypes.Add(typeof(FSW.Property));
                _knownTypes.Add(typeof(FSW.PropertyBag));
                //knownTypes.Add(typeof(FSW..PropertyBagCollection));
            }

            return _knownTypes;
        }
    }

    [ServiceKnownType("GetKnownTypes", typeof(FSWServiceKnownTypeHelper))]
    [ServiceContract(CallbackContract = typeof(IFSWServiceCallback))]
    public interface IFSWService
    {
        /// <summary>
        /// Used to connect to the service and retrieve a session key
        /// </summary>
        /// <param name="providerKey">The provider key mapped to the providers table</param>
        /// <param name="deviceCode">A unique device assigned to the provider</param>
        /// <returns>Returns a unique identifier for the new or existing session</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        Guid Connect(string providerKey, int deviceCode);

        /// <summary>
        /// Used to disconnect from the service and close the session.
        /// </summary>
        /// <param name="sessionKey">The session key returned from the connect call</param>
        /// <returns>Returns false when the session is no longer open, otherwise returns true.
        /// A FaultException will be raised if any errors occur during execution.</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool Disconnect(Guid sessionKey);

        /// <summary>
        /// Used to keep a session alive
        /// </summary>
        /// <param name="sessionKey">The session key returned from the connect call</param>
        /// <returns>Returns false when the session is no longer open, otherwise returns true</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool KeepAlive(Guid sessionKey);

        /* These should be performed through Execute calls
         * 
        /// <summary>
        /// Used for clients to subscribe to callback channels
        /// </summary>
        /// <param name="sessionKey">The session key returned from the connect call</param>
        /// <param name="feeds">A string array containing callback feed names</param>
        /// <returns>The number of feeds that were subscribed to</returns>
        /// <remarks>This method is designed to be used for subscription to server side message queues such as watch messages,
        /// service control notifications etc. It is not required to be called for asynchronous method excution.</remarks>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        int Subscribe(Guid sessionKey, string[] feeds);

        /// <summary>
        /// Used for clients to unsubscribe to callback channels
        /// </summary>
        /// <param name="sessionKey">The session key returned from the connect call</param>
        /// <param name="feeds">A string array containing callback feed names</param>
        /// <returns>The number of feeds that were unsubscribed from</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        int Unsubscribe(Guid sessionKey, string[] feeds);

         * */

        /// <summary>
        /// Synchronous packet request execution.
        /// </summary>
        /// <param name="command">A request packet</param>
        /// <returns>A response packet</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        Packet Execute(Packet command);

        /// <summary>
        /// Asynchronous packet request execution. Packets are returned through the client callback delegate.
        /// This method may also be used for command requates that don't require any response.
        /// </summary>
        /// <param name="command">A request packet</param>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void ExecuteAsync(Packet packet);
    }
}
