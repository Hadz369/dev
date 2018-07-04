using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ruby.Data
{
    /// <summary>
    /// Event delegate for statistic information
    /// </summary>
    /// <param name="stats">Collection of statistics</param>
    public delegate void StatisticsEventHandler(StatisticsBase stats);

    /// <summary>
    /// Statistics Base Class
    /// </summary>
    public abstract class StatisticsBase
    {
        /// <summary>
        /// Protected dictionary of statistic metrics related to a statistic type
        /// </summary>
        protected Dictionary<string, object> _stats = new Dictionary<string, object>();

        /// <summary>
        /// Public dictionary of statistic metrics related to a statistic type
        /// </summary>
        public Dictionary<string, object> Statistics { get { return _stats; } }

        /// <summary>
        /// Abstract ToString implementation to force overriding
        /// </summary>
        /// <returns></returns>
        public abstract new string ToString();
    }
}
