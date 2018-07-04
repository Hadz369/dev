using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using Metro;

namespace Metro.Data
{
    /// <summary>
    /// A collection of Channel statistics
    /// </summary>
    public class ChannelProxyStatistics : StatisticsBase
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Statistic type</param>
        /// <param name="id">Channel Id</param>
        /// <param name="inUse">Channel is in use</param>
        /// <param name="created">DateTime the channel was created</param>
        /// <param name="lastUsed">Last DateTime the channel was used</param>
        /// <param name="reuseCount">How many times the channel has been reused</param>
        /// <param name="execCount">How many commands have been executed</param>
        public ChannelProxyStatistics(string type, int id, bool inUse, DateTime created, DateTime lastUsed, int reuseCount, int execCount)
        {
            _stats.Add("Type", type);
            _stats.Add("Id", id);
            _stats.Add("InUse", inUse);
            _stats.Add("Duration", (lastUsed.Subtract(created)).TotalMilliseconds);
            _stats.Add("Created", created);
            _stats.Add("LastUsed", lastUsed);
            _stats.Add("ReuseCount", reuseCount);
            _stats.Add("ExecCount", execCount);
        }

        /// <summary>
        /// Build a string representation of the statistic collection
        /// </summary>
        /// <returns>Formatted statistics string</returns>
        public override string ToString()
        {
            string s = "";

            foreach (KeyValuePair<string, object> kvp in _stats)
                s += String.Format("{0}{1}={2}", (s != "" ? ", " : ""), kvp.Key, kvp.Value);

            return s;
        }
    }

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
            Tracer.Debug(String.Format("Channel created: Type={0}, Guid={1}", _channel.ToString(), _guid.ToString()));
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
                Tracer.Debug(String.Concat("Closing channel. Guid=", _guid.ToString()));
                ((IClientChannel)_channel).Close();
                ((IDisposable)_channel).Dispose();
            }
            else
            {
                Tracer.Debug(String.Concat("Aborting channel. Guid=", _guid.ToString()));
                ((IClientChannel)_channel).Abort();
                ((IDisposable)_channel).Dispose();
            }
        }
    }
}
