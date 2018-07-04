using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Ruby.Core;

namespace Ruby.Data
{
    public class DbConnectionBroker : IDisposable
    {
        Object _locker = new Object();
        int _timerInterval = 1000;
        Timer _timer;
        int _nextConnectionId = 1000;
        int _statsTicks = 0, _statsTrigger = 10, _cleanupTicks = 0, _cleanupTrigger = 30, _discardMinutes = 1;

        Dictionary<string, string> _registered = new Dictionary<string, string>();

        Dictionary<string, List<DbConnection>> _connections = new Dictionary<string, List<DbConnection>>();

        #region Singleton Initialisation

        private static DbConnectionBroker _instance = null;

        private static Object initlock = new Object();

        private DbConnectionBroker()
        {
            _timer = new Timer(OnTimer);
            _timer.Change(_timerInterval, _timerInterval);
        }

        private static DbConnectionBroker GetInstance()
        {
            if (_instance == null)
            {
                lock (initlock)
                {
                    if (_instance == null)
                    {
                        _instance = new DbConnectionBroker();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Returns the singleton instance of the DbConnectionBroker class
        /// </summary>
        public static DbConnectionBroker Instance { get { return GetInstance(); } }

        #endregion

        /// <summary>
        /// Register a connection string with the connection broker
        /// </summary>
        /// <param name="key">The identifying key of the connection string owner</param>
        /// <param name="constring">The connection string</param>
        /// <returns>True = Success, False = Failure</returns>
        public bool Register(string key, string constring)
        {
            bool registered = false;

            if (!_registered.ContainsKey(key))
            {
                _registered.Add(key, constring);
                registered = true;
            }

            return registered;
        }

        /// <summary>
        /// Deregister a registered connection string
        /// </summary>
        /// <param name="key">The identifying key of the string to be removed</param>
        /// <returns>True = Success, False = Failure</returns>
        public bool DeRegister(string key)
        {
            bool registered = false;

            if (_registered.ContainsKey(key))
            {
                _registered.Remove(key);
            }

            return registered;
        }

        /// <summary>
        /// Status timer callback
        /// <remarks>
        /// Check the connection list every 5 seconds and remove any entries that are not in use
        /// where their last activity was more than 1 minute ago
        /// </remarks>
        /// </summary>
        /// <param name="stateInfo">Callback data</param>
        void OnTimer(object stateInfo)
        {
            _statsTicks++;
            _cleanupTicks++;

            if (_cleanupTicks >= _cleanupTrigger)
            {
                Cleanup();
            }

            if (_statsTicks >= _statsTrigger)
            {
                GetStats();
            }
        }

        /// <summary>
        /// Remove connections from the pool that have been inactive for more than a minute
        /// </summary>
        void Cleanup()
        {
            _cleanupTicks = 0;

            // Lock the _locker object to prevent list access during cleanup
            lock (_locker)
            {
                if (_connections.Count > 0)
                {
                    Tracer.Info("Connection cleanup initialised");

                    List<string> removeKeys = new List<string>();

                    foreach (KeyValuePair<string, List<DbConnection>> kvp in _connections)
                    {
                        List<DbConnection> removeCons = new List<DbConnection>();

                        foreach (DbConnection c in kvp.Value)
                        {
                            if (!c.InUse && c.LastUsed.AddMinutes(_discardMinutes) < DateTime.Now)
                            {
                                removeCons.Add(c);
                            }
                        }

                        // Remove the items
                        foreach (DbConnection c in removeCons)
                        {
                            kvp.Value.Remove(c);
                            c.Dispose();
                        }

                        // Remove the kvp when the list has no items
                        if (kvp.Value.Count == 0) removeKeys.Add(kvp.Key);
                    }

                    // Remove the items with no existing connections
                    foreach (string key in removeKeys) _connections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Gather connection statistics
        /// </summary>
        void GetStats()
        {
            _statsTicks = 0;

            foreach (KeyValuePair<string, List<DbConnection>> kvp in _connections)
            {
                foreach (DbConnection c in kvp.Value)
                {
                    c.SendStats();
                }
            }
        }

        /// <summary>
        /// Get an available connection from the pool
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DbConnection GetConnection(string key)
        {
            DbConnection con = null;

            // Create a lock so that it won't conflict with the array cleanup
            lock (_locker)
            {
                if (_registered.ContainsKey(key))
                {
                    if (_connections.ContainsKey(key))
                    {
                        foreach (DbConnection c in _connections[key])
                        {
                            if (!c.InUse)
                            {
                                con = c;
                                con.Initialise();
                                Tracer.Debug(String.Concat("Existing connection reused: Id=", con.Id));
                                break;
                            }
                        }

                        if (con == null)
                        {
                            // Get the connection string from the registered list and create a new one
                            con = new DbConnection(_registered[key], _nextConnectionId++);
                            con.StatisticsEvent += con_StatisticsEvent;

                            // Add the connection to the connection manager for the registered type
                            _connections[key].Add(con);
                            Tracer.Debug(String.Concat("New connection created: Id=", con.Id));
                        }
                    }
                    else
                    {
                        // Get the connection string from the registered list and create a new one
                        con = new DbConnection(_registered[key], _nextConnectionId++);
                        con.StatisticsEvent += con_StatisticsEvent;

                        // Add the connection to the connection manager into a new guid group
                        _connections.Add(key, new List<DbConnection> { con });
                        Tracer.Debug(String.Concat("New connection/group created: Guid=", con.Id));
                    }
                }
            }

            return con;
        }

        /// <summary>
        /// Log the statistics to the trace logs
        /// </summary>
        /// <param name="stats"></param>
        void con_StatisticsEvent(StatisticsBase stats)
        {
            Tracer.Always("Statistics: " + stats.ToString());
        }

        /// <summary>
        /// Build the connection statistics summary
        /// </summary>
        /// <returns>A formatted summary string</returns>
        public string GetSummary()
        {
            string s = "";

            foreach (KeyValuePair<string, List<DbConnection>> kvp in _connections)
            {
                s = String.Format("{0}\r\nConnectionKey={1}", s, kvp.Key);

                foreach (DbConnection con in kvp.Value)
                {
                    s = String.Format("{0}\r\n> Id={1}, InUse={2}, LastUsed={3}", s, con.Id, con.InUse, con.LastUsed);
                }
            }

            return s;
        }

        /// <summary>
        /// Stop the timer, close all connections and dispose of everything
        /// </summary>
        public void Dispose()
        {
            Tracer.Debug("DbConnectionBroker disposing");

            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Dispose();
            }

            foreach (KeyValuePair<string, List<DbConnection>> kvp in _connections)
            {
                foreach (DbConnection con in kvp.Value)
                {
                    con.SendStats();

                    con.StatisticsEvent -= con_StatisticsEvent;
                    con.Dispose();
                }
            }

            _connections = null;
        }
    }
}
