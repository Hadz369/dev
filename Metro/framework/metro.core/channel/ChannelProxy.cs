using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;

namespace Metro
{
    /// <summary>
    /// Channel Proxy
    /// <remarks>
    /// The channel proxy provides a wrapper around a Channel to provide simple disposal.
    /// </remarks>
    /// </summary>
    public class ChannelProxy<T> : IDisposable
    {
        Guid _guid;
        T _channel;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="channel">Channel</param>
        public ChannelProxy(T channel)
        {
            _guid = Guid.NewGuid();
            _channel = channel;
        }

        /// <summary>
        /// Channel identifier
        /// </summary>
        public Guid Guid { get { return _guid; } }

        /// <summary>
        /// IClientChannel object
        /// </summary>
        public T Channel { get { return _channel; } }

        /// <summary>
        /// Close the channel and dispose of it
        /// </summary>
        public void Dispose()
        {
            if (((IClientChannel)_channel).State != CommunicationState.Faulted)
            {
                ((IClientChannel)_channel).Close();
                ((IDisposable)_channel).Dispose();
            }
            else
            {
                ((IClientChannel)_channel).Abort();
                ((IDisposable)_channel).Dispose();
            }
        }
    }
}
