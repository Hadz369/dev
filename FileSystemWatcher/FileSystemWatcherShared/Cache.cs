using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSW
{
    public static class Cache
    {
        static SessionCollection _sessions = new SessionCollection();
        static SubscriberChannelCollection _subscriberChannels = new SubscriberChannelCollection();

        /// <summary>
        /// Contains a list of sessions
        /// </summary>
        public static SessionCollection Sessions { get { return _sessions; } }
        /// <summary>
        /// Contains a list of dynamically added subscriber collections
        /// </summary>
        public static SubscriberChannelCollection SubscriberChannels { get { return _subscriberChannels; } }
    }
}
