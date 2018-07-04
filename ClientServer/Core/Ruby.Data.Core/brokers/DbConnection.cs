﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Ruby.Core;

namespace Ruby.Data
{
    /// <summary>
    /// A collection of DbConnection statistics
    /// </summary>
    public class DbConnectionStatistics : StatisticsBase
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Statistic type</param>
        /// <param name="id">Connection Id</param>
        /// <param name="inUse">Connection is in use</param>
        /// <param name="created">DateTime the connection was created</param>
        /// <param name="lastUsed">Last DateTime the connection was used</param>
        /// <param name="reuseCount">How many times the connection has been reused</param>
        /// <param name="execCount">How many commands have been executed</param>
        public DbConnectionStatistics(string type, int id, bool inUse, DateTime created, DateTime lastUsed, int reuseCount, int execCount)
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
    /// Database connection object
    /// </summary>
    public class DbConnection : IDisposable
    {
        /// <summary>
        /// Statistics event handler
        /// </summary>
        public event StatisticsEventHandler StatisticsEvent;

        bool _inUse = true;
        DateTime _created = DateTime.Now, _lastUsed = DateTime.Now;
        SqlConnection _con;
        int _reuseCount = 0, _execCount = 0;
        int _connectionId = 0;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="connectionId">Id for the connection generated by the broker</param>
        public DbConnection(string connectionString, int connectionId)
        {
            _connectionId = connectionId;
            _con = new SqlConnection(connectionString);
            _con.Open();
        }

        /// <summary>
        /// Connection Id
        /// </summary>
        public int Id { get { return _connectionId; } }
        /// <summary>
        /// Connection is currently in use
        /// </summary>
        public bool InUse { get { return _inUse; } set { _inUse = value; } }
        /// <summary>
        /// Last time the connection was used
        /// </summary>
        public DateTime LastUsed { get { return _lastUsed; } }
        /// <summary>
        /// SqlConnection object
        /// </summary>
        public SqlConnection SqlConnection { get { return _con; } }

        /// <summary>
        /// Signal the connections to send statistics
        /// </summary>
        public void SendStats()
        {
            if (StatisticsEvent != null)
            {
                StatisticsEvent(new DbConnectionStatistics(this.GetType().Name, _connectionId, _inUse, _created, _lastUsed, _reuseCount, _execCount));
            }
        }

        /// <summary>
        /// Set the last used date of the connection
        /// </summary>
        public void SetUsed()
        {
            _execCount++;
            _lastUsed = DateTime.Now;
        }

        /// <summary>
        /// Initialise an existing connection
        /// </summary>
        public void Initialise()
        {
            Tracer.Debug(String.Concat("Initialising existing connection. Id=", _connectionId));
            _inUse = true;
            _reuseCount++;
            _lastUsed = DateTime.Now;
        }

        /// <summary>
        /// Close the database connection and dispose of it
        /// </summary>
        public void Dispose()
        {
            Tracer.Debug(String.Format("Disposing Connection: Id={0}, InUse={1}, LastUsed={2}", 
                _connectionId, _inUse, _lastUsed));

            if (_con.State != ConnectionState.Closed)
            {
                Tracer.Debug(String.Concat("Closing connection. Id=", _connectionId));
                _con.Close();
            }

            _con.Dispose();
        }
    }
}
