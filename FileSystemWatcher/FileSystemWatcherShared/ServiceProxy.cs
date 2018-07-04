using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;

namespace FSW
{
    public class ServiceProxy<T> : IDisposable
    {
        Guid _guid;
        T _channel;

        public ServiceProxy(T channel)
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
