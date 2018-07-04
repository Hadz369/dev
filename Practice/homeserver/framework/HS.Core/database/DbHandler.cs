using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace HS
{
    /// <summary>
    /// DbHandler Class
    /// <remarks>
    /// This is the DbHandler class
    /// </remarks>
    /// </summary>
    public class DbHandler : IDisposable
    {
        bool _isTransaction = false;
        int _uid, _mid;

        DbConnection _con;
        SqlTransaction _tran;

        #region Initialisation

        /// <summary>
        /// Main entry point for the class
        /// </summary>
        /// <param name="key">Key used to retrieve a connection from the broker</param>
        public DbHandler(string key)
        {
            try
            {
                DbConnectionBroker cb = DbConnectionBroker.Instance;
                _con = cb.GetConnection(key);
            }
            catch (Exception ex)
            {
                Tracer.Error("Error during DbHandler initialisation.", ex);
                if (_con != null) _con.IsFaulted = true;
            }
        }

        /// <summary>
        /// Main entry point for the class with person and module id options
        /// </summary>
        /// <param name="key">Key used to retrieve a connection from the broker</param>
        public DbHandler(string key, int uid, int mid)
        {
            try
            {
                DbConnectionBroker cb = DbConnectionBroker.Instance;
                _con = cb.GetConnection(key);

                _uid = uid;
                _mid = mid;
            }
            catch (Exception ex)
            {
                Tracer.Error("Error during DbHandler initialisation.", ex);
                if (_con != null) _con.IsFaulted = true;
            }
        }

        #endregion

        #region Properties

        public int UID { get { return _uid; } }
        public int MID { get { return _mid; } }

        public bool IsFaulted { get { return _con.IsFaulted; } }

        #endregion

        #region Connection Handling

        /// <summary>
        /// Sql connection object
        /// </summary>
        public SqlConnection Connection { get { return _con.SqlConnection; } }
        
        /// <summary>
        /// The DbHandler is performing a transaction
        /// </summary>
        public bool IsTransaction { get { return _isTransaction; } }
        /// <summary>
        /// The transaction currently being performed
        /// </summary>
        public SqlTransaction Transaction { get { return _tran; } }

        /// <summary>
        /// Set the underlying DbConnection to the faulted state so that it won't be reused
        /// </summary>
        public void SetFaulted()
        {
            Tracer.Debug(String.Concat("DbConnection faulted: Id=", _con.Id));
            _con.IsFaulted = true;
        }

        /// <summary>
        /// Connect to the database
        /// </summary>
        /// <returns></returns>
        bool Connect()
        {
            bool connected = (_con.SqlConnection.State == ConnectionState.Open);

            if (!connected)
            {
                try
                {
                    Tracer.Debug("Connecting to the database.");

                    _con.SqlConnection.Open();
                    connected = true;
                }
                catch(Exception ex)
                {
                    Tracer.Error("Error opening database connection.", ex);
                    connected = false;
                    SetFaulted();
                }
            }

            return connected;
        }

        /// <summary>
        /// Disconnect from the database
        /// </summary>
        /// <returns></returns>
        bool Disconnect()
        {
            bool connected = !(_con.SqlConnection.State == ConnectionState.Closed);

            if (connected)
            {
                try
                {
                    Tracer.Debug("Disconnecting from the database.");

                    _con.SqlConnection.Close();
                    connected = false;
                }
                catch (Exception ex)
                {
                    Tracer.Error("Error closing the database connection.", ex);
                    connected = true;
                }
            }

            return connected;
        }

        #endregion

        #region Transaction Handling

        /// <summary>
        /// Start a new transaction
        /// </summary>
        public void BeginTransaction()
        {
            _isTransaction = true;
            _tran = _con.SqlConnection.BeginTransaction();
        }

        /// <summary>
        ///  Commit the transaction
        /// </summary>
        public void Commit()
        {
            try 
            { 
                _tran.Commit(); 
            }
            catch (Exception ex)
            {
                Tracer.Error("Error commiting transaction.", ex);
                this.RollBack();
            }
            
            _isTransaction = false;
        }

        /// <summary>
        /// Rollback the transaction
        /// </summary>
        public void RollBack()
        {
            try
            {
                _tran.Rollback();
            }
            catch (Exception ex)
            {
                Tracer.Error("Error rolling back transaction.", ex);
            }

            _isTransaction = false;
        }

        #endregion

        #region SQL Execution

        /// <summary>
        /// Get a new SQLCommand
        /// </summary>
        /// <param name="commandType">Command Type</param>
        /// <param name="commandText">Command Text</param>
        /// <returns>SQLCommand</returns>
        public SqlCommand GetCommand(CommandType commandType, string commandText)
        {
            SqlCommand cmd = new SqlCommand(commandText, _con.SqlConnection);
            cmd.CommandType = commandType;

            if (_isTransaction) cmd.Transaction = _tran;

            return cmd;
        }

        /// <summary>
        /// Execute an SQL Command and return a single recordset
        /// </summary>
        /// <param name="cmd">SQLCommand to be executed</param>
        /// <returns>DataTable</returns>
        public DataTable Execute(SqlCommand cmd)
        {
            return Execute(cmd, "");
        }

        /// <summary>
        /// Execute an SQL Command and return a single recordset
        /// </summary>
        /// <param name="cmd">SQLCommand to be executed</param>
        /// <param name="tableName">The name of the resulting table</param>
        /// <returns>DataTable</returns>
        public DataTable Execute(SqlCommand cmd, string tableName)
        {
            Connect();

            if (tableName.Trim() == String.Empty) tableName = "Undefined";

            DataTable dt = new DataTable(tableName);
            dt.Load(cmd.ExecuteReader());

            _con.SetUsed();

            return dt;
        }

        /// <summary>
        /// Execute an SQL Command and return a dataset
        /// <remarks>
        /// This method should be used when calling a stored procedure that 
        /// may return multiple result sets
        /// </remarks>
        /// </summary>
        /// <param name="cmd">SQLCommand to be executed</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(SqlCommand cmd)
        {
            Connect();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            _con.SetUsed();

            return ds;
        }

        /// <summary>
        /// Execute an SQL Command where no result set is returned
        /// </summary>
        /// <param name="cmd">SQLCommand to be executed</param>
        /// <returns>Either the number or rows affected or the return value parameter</returns>
        public object ExecuteNonQuery(SqlCommand cmd)
        {
            Connect();

            object rc = cmd.ExecuteNonQuery();

            foreach (SqlParameter p in cmd.Parameters)
            {
                if (p.Direction == ParameterDirection.ReturnValue)
                {
                    rc = p.Value;
                    break;
                }
            }

            _con.SetUsed();

            return rc;
        }
        
        /// <summary>
        /// Execute an SQL Command and return a single scalar object
        /// </summary>
        /// <param name="cmd">SQLCommand to be executed</param>
        /// <returns>The scalar value returned from the SQL execution</returns>
        public object ExecuteScalar(SqlCommand cmd)
        {
            Connect();

            object o = cmd.ExecuteScalar();

            _con.SetUsed();

            return o;
        }

        #endregion

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose()
        {
            Tracer.Debug("DbHandler disposing");

            if (_con != null) _con.InUse = false;
        }
    }

    /// <summary>
    /// A dictionary used to hold a list of parameter names and values
    /// </summary>
    public class Parms : Dictionary<string, object>
    {
        /// <summary>
        /// Get a parameter from the dictionary with the provided key
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <returns>The parameter value</returns>
        public object GetParm(string key)
        {
            object val = null;

            if (this.ContainsKey(key)) val = this[key];

            return val;
        }
    }
}
