using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using System.ServiceModel;

namespace Metro
{
    public delegate void ChannelFactoryMessageHandler(TraceLevel level, string message, Exception ex);

    /// <summary>
    /// A singleton class used to hold a list of channel factories for a given interface and 
    /// allocate channel proxies when requested
    /// </summary>
    /// <typeparam name="T">The interface type of factories being managed</typeparam>
    public class ChannelBroker<T> : IDisposable
    {
        public event ChannelFactoryMessageHandler MessageEvent;

        Dictionary<string, ChannelFactory<T>> _registered = new Dictionary<string, ChannelFactory<T>>();

        Dictionary<string, int> _packetCounters = new Dictionary<string, int>();

        #region Singleton Initialisation

        private static ChannelBroker<T> _instance = null;

        private static Object initlock = new Object();

        private ChannelBroker()
        {
        }

        private static ChannelBroker<T> GetInstance()
        {
            if (_instance == null)
            {
                lock (initlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ChannelBroker<T>();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Returns the singleton instance of the channel broker class
        /// </summary>
        public static ChannelBroker<T> Instance { get { return GetInstance(); } }

        #endregion

        private void SendMessage(TraceLevel level, string message, Exception ex)
        {
            if (MessageEvent != null)
            {
                MessageEvent(level, message, ex);
            }
        }

        /// <summary>
        /// Register a channel factory
        /// <remarks>
        /// When the register function is called and the key does not exist in the collection
        /// of registered channel factories then the key/factory pair is added to the list 
        /// and a value of true is returned, otherwise no action is performed and a value 
        /// of false is returned.
        /// </remarks>
        /// </summary>
        /// <param name="key">A unique identifying key for the channel factory</param>
        /// <param name="factory">The channel factory to add to the collection</param>
        /// <returns>True = Success, False = Failure</returns>
        public bool Register(string key, ChannelFactory<T> factory)
        {
            bool registered = false;

            SendMessage(TraceLevel.Info,
                String.Format(
                "Registering channel factory. Key={0}, Uri={1}", key, factory.Endpoint.Address.Uri.OriginalString),
                null);

            if (!_registered.ContainsKey(key))
            {
                _registered.Add(key, factory);
                _packetCounters.Add(key, 0);
                
                registered = true;
                                
                SendMessage(TraceLevel.Info, 
                    "Channel factory registered", null);
            }
            else
            {
                SendMessage(TraceLevel.Info, 
                    "Channel factory already registered", null);
            }

            return registered;
        }

        /// <summary>
        /// Deregister a registered channel factory
        /// <remarks>
        /// When a a registered channel factory is no longer required it should be
        /// deregistered which will remove it from the collection and free all resources.
        /// If a factory exists for the given key it will be closed and removed and a value 
        /// of true will be returned to the caller otherwise a value of false will be returned.
        /// </remarks>
        /// </summary>
        /// <param name="key">The key of the factory to be removed</param>
        /// <returns>True = Success, False = Failure</returns>
        public bool DeRegister(string key)
        {
            bool deregistered = false;

            if (_registered.ContainsKey(key))
            {
                try { _registered[key].Close(); }
                catch (Exception ex) { SendMessage(TraceLevel.Error, "Error closing channel factory", ex); }

                _registered.Remove(key);
                _packetCounters.Remove(key);

                deregistered = true;
            }

            return deregistered;
        }

        /// <summary>
        /// Get a new channel proxy from the factory identified by the given key.
        /// <remarks>
        /// Calling this method with an invalid key will result in an exception being thrown.
        /// </remarks>
        /// </summary>
        /// <param name="key">The unique key of the factory to use</param>
        /// <returns>A new channel proxy</returns>
        public ChannelProxy<T> GetProxy(string key)
        {
            if (_registered.ContainsKey(key) && _registered[key].State != CommunicationState.Faulted)
            {
                return new ChannelProxy<T>((_registered[key]).CreateChannel());
            }
            else
            {
                throw new Exception(String.Concat("Channel factory not registered or in the faulted state. Key=", key));
            }
        }

        public int GetCounter(string key)
        {
            if (_packetCounters.ContainsKey(key))
                return ++_packetCounters[key];
            else
                return 0;
        }

        /// <summary>
        /// Close all chammel factories and dispose of everything
        /// </summary>
        public void Dispose()
        {
            SendMessage(TraceLevel.Info, "ChannelBroker disposing", null);

            foreach (KeyValuePair<string, ChannelFactory<T>> kvp in _registered)
            {
                SendMessage(TraceLevel.Info, 
                    String.Format("Deregistering channel factory. Key={0}, Uri={1}", 
                        kvp.Key, kvp.Value.Endpoint.Address.Uri.OriginalString), null);

                try
                {
                    if (kvp.Value.State != CommunicationState.Faulted)
                    {
                        if (kvp.Value.State == CommunicationState.Opened)
                            kvp.Value.Close();
                    }
                }
                catch (Exception ex)
                {
                    SendMessage(TraceLevel.Error, "Error deregistering channel factory.", ex);
                }
            }

            _registered = null;
        }
    }
}
