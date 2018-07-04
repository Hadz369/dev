using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using Alma.Core;

namespace Alma.Data
{
    public class DbHandler
    {
        bool _isTransaction = false;

        SqlConnection _con;
        SqlTransaction _tran;
        System.Threading.Timer _timer;
        DateTime _lastDbCallStart, _lastDbCallEnd;
        TraceLevel _traceLevel = TraceLevel.Error;
        int _inactivityTimeout = 180000;

        TableInfoCollection _tableInfo;

        #region Initialisation

        public DbHandler(string conString)
        {
            Init(conString, _traceLevel);
        }

        public DbHandler(string conString, TraceLevel traceLevel)
        {
            Init(conString, traceLevel);
        }

        void Init(string conString, TraceLevel traceLevel)
        {
            try
            {
                _traceLevel = traceLevel;
                _timer = new System.Threading.Timer(TimerCallback);

                _con = new SqlConnection(conString);

                _tableInfo = new TableInfoCollection();
            }
            catch (Exception ex)
            {
                Tracer.WriteLine(new TracerData("Error during DbHandler initialisation.", ex));
            }
        }

        #endregion

        public int InactivityTimeout { get { return _inactivityTimeout; } set { _inactivityTimeout = value; } }

        #region Connection Handling

        bool Connect()
        {
            bool connected = (_con.State == ConnectionState.Open);

            if (!connected)
            {
                try
                {
                    Tracer.WriteLine(new TracerData(TraceLevel.Verbose, "Connecting to the database."));

                    _con.Open();
                    connected = true;

                    StartTimer();
                }
                catch(Exception ex)
                {
                    Tracer.WriteLine(new TracerData("Error opening database connection.", ex));
                    connected = false;

                    StopTimer();
                }
            }

            return connected;
        }

        bool Disconnect()
        {
            bool connected = !(_con.State == ConnectionState.Closed);

            if (connected)
            {
                try
                {
                    if (_traceLevel == TraceLevel.Verbose)
                        Tracer.WriteLine(new TracerData("Disconnecting from the database."));

                    _con.Close();
                    connected = false;

                    StopTimer();
                }
                catch (Exception ex)
                {
                    Tracer.WriteLine(new TracerData("Error closing the database connection.", ex));
                    connected = true;

                    StartTimer();
                }
            }

            return connected;
        }

        void TimerCallback(object stateInfo)
        {
            if (_lastDbCallEnd < DateTime.Now.AddMinutes(-3))
            {
                if (_traceLevel == TraceLevel.Verbose)
                    Tracer.WriteLine(new TracerData("Connection inactivity timer exceeded."));

                Disconnect();
                StopTimer();
            }
        }

        void StartTimer()
        {
            _timer.Change(1000, 1000);
        }

        void StopTimer()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        #region Transaction Handling

        public void BeginTransaction()
        {
            _isTransaction = true;
            _tran = _con.BeginTransaction();
        }

        public void Commit()
        {
            try 
            { 
                _tran.Commit(); 
            }
            catch (Exception ex)
            {
                Tracer.WriteLine(new TracerData("Error commiting transaction.", ex));
                this.RollBack();
            }
            
            _isTransaction = false;
        }

        public void RollBack()
        {
            try
            {
                _tran.Rollback();
            }
            catch (Exception ex)
            {
                Tracer.WriteLine(new TracerData("Error rolling back transaction.", ex));
            }

            _isTransaction = false;
        }

        #endregion

        #region SQL Execution

        public SqlCommand GetCommand(CommandType commandType, string commandText)
        {
            SqlCommand cmd = new SqlCommand(commandText, _con);
            cmd.CommandType = commandType;

            if (_isTransaction) cmd.Transaction = _tran;

            return cmd;
        }

        public DataTable Execute(CommandType commandType, string sql, Parms parms)
        {
            SqlCommand cmd = GetCommand(commandType, sql);

            if (parms != null)
            {
                foreach (KeyValuePair<string, object> kvp in parms)
                {
                    cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }
            }

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            return dt;
        }

        public int ExecuteNonQuery(CommandType commandType, string sql, Parms parms)
        {
            SqlCommand cmd = GetCommand(commandType, sql);

            if (parms != null)
            {
                foreach (KeyValuePair<string, object> kvp in parms)
                {
                    cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }
            }

            return cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(CommandType commandType, string sql, Parms parms)
        {
            SqlCommand cmd = GetCommand(commandType, sql);

            if (parms != null)
            {
                foreach (KeyValuePair<string, object> kvp in parms)
                {
                    cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }
            }

            return cmd.ExecuteScalar();
        }

        #endregion

        #region Record Management

        /// <summary>
        /// Reads the column schema for a given schema and table in the current 
        /// database used by the SqlConnection.
        /// Looks in columnInfo list first. If not found, reads from the database
        /// and adds it to the list for the next call.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private TableInfo GetTableInfo(string table)
        {
            TableInfo tableInfo = SplitTableName(table), ti = null;

            // Try to read it from the list first
            var t = (from TableInfo i in _tableInfo
                     where i.DatabaseName == tableInfo.DatabaseName
                    && i.SchemaName == tableInfo.SchemaName
                     && i.TableName == tableInfo.TableName
                     select ti).First();

            ti = t as TableInfo;

            // Load all of the columns if no existing table info objects exists
            // in the collection
            if (ti == null)
            {
                string sql = "" +
                    "select " +
                    " ordinal_position as column_id, " +
                    " column_name as name, " +
                    " case when is_nullable = 'YES' then 1 else 0 end as is_nullable, " +
                    " case when column_default is not null then 1 else 0 end as has_default " +
                    "from INFORMATION_SCHEMA.columns " +
                    "where table_catalog = @database " +
                    "and table_schema = @schema " +
                    "and table_name = @table";

                Parms p = new Parms();
                p.Add("database", tableInfo.DatabaseName);
                p.Add("schema", tableInfo.SchemaName);
                p.Add("table", tableInfo.TableName);

                DataTable dt = Execute(CommandType.Text, sql, p);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        ColumnInfo ci = new ColumnInfo(r);
                        // Add it to the list for next time
                        tableInfo.Columns.Add(ci);
                    }
                }
            }

            return tableInfo;
        }

        /// <summary>
        ///  Used to split a table name for the GetColumnInfo procedure
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private TableInfo SplitTableName(string table)
        {
            TableInfo tableInfo = null;

            string[] data = new string[3];
            string[] str = table.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (str.Length == 1)
            {
                tableInfo = new TableInfo(_con.Database, "dbo", str[0]);
            }
            else if (str.Length == 2)
            {
                tableInfo = new TableInfo(_con.Database, str[0], str[1]);
            }
            else if (str.Length == 3)
            {
                tableInfo = new TableInfo(str[0], str[1], str[2]);
            }

            return tableInfo;
        }

        bool ValidateParameters(string table, Parms parms)
        {
            bool valid = false;

            TableInfo ti = GetTableInfo(table);

            if (ti != null)
            {
                foreach (KeyValuePair<string, object> kvp in parms)
                {
/*
 * var col= (from ColumnInfo ci in kvp.Value
                                  where ci.DatabaseName == ti.DatabaseName
                                  && ci.SchemaName == ti.SchemaName
                                  && ci.TableName == ti.TableName
                                  select ci).First();

                    colInfo = col as ColumnInfo;
                    */
                }
            }

            return valid;
        }

        public DataRow GetRecord(string tableName, string keyName, int id)
        {
            string sql = "", pname = "";

            Parms parms = new Parms();

            pname = String.Concat("@", keyName.Trim());
            parms.Add(pname, id);

            sql = String.Format("select * from {0} where {1}={2}", tableName, keyName, pname);

            DataTable dt = Execute(CommandType.Text, sql, parms);

            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }

        public int InsertRecord(string tableName, Parms parms)
        {
            string sql = "", cols = "", vals = "";

            int id = 0;

            foreach (string key in parms.Keys)
            {
                cols += String.Concat(cols == "" ? "" : ", ", key);
                vals += String.Concat(vals == "" ? "@" : ", @", key);
            }

            sql = String.Format("insert into {0} ({1}) values({2}); select scope_identity();",
                tableName, cols, vals);

            object rc = ExecuteScalar(CommandType.Text, sql, parms);

            if (rc != null) id = Convert.ToInt32(rc);

            return id;
        }

        public int UpdateRecord(string tableName, string keyName, int id, Parms parms)
        {
            string sql = "";
            int rowsAffected = 0;

            if (parms.Count > 0)
            {
                // Read existing first
                DataRow row = GetRecord(tableName, keyName, id);

                if (row != null)
                {
                    string fields = PrepareUpdateFields(row, parms);

                    if (fields != "")
                    {
                        parms.Add(keyName, id);

                        sql = String.Format("update {0} set {1} where {2}=@{2}", tableName, fields, keyName);

                        rowsAffected = ExecuteNonQuery(CommandType.Text, sql, parms);
                    }
                }
            }
            else
            {
                rowsAffected = -1;
            }

            return rowsAffected;
        }


        public string PrepareUpdateFields(DataRow row, Parms parms)
        {
            string sql = "";
            bool updatedIsNull = false;

            foreach (KeyValuePair<string, object> kvp in parms)
            {
                // If a null value is supplied for the updated column then default it to now
                if (String.Compare(kvp.Key, "updated", true) == 0 && kvp.Value == null)
                    updatedIsNull = true;

                if (!row[kvp.Key].Equals(kvp.Value))
                    sql += String.Concat(sql == "" ? "" : ", ", String.Format("{0}=@{0}", kvp.Key));
            }

            if (updatedIsNull)
                parms["Updated"] = DateTime.Now;

            return sql;
        }

        #endregion
    }

    public class Parms : Dictionary<string, object>
    {
        public Parms()
        {
        }

        public object GetParm(string key)
        {
            object val = null;

            if (this.ContainsKey(key)) val = this[key];

            return val;
        }
    }

    class TableInfoCollection : List<TableInfo> { }

    /// <summary>
    /// </summary>
    class TableInfo
    {
        string _dbName, _schemaName, _tableName, _columnName;
        List<ColumnInfo> _columnInfo;

        public TableInfo(string database, string schema, string table)
        {
            _dbName = database;
            _schemaName = schema;
            _tableName = table;

            _columnInfo = new List<ColumnInfo>();
        }

        public string DatabaseName { get { return _dbName; } }
        public string SchemaName { get { return _schemaName; } }
        public string TableName { get { return _tableName; } }
        public List<ColumnInfo> Columns { get { return _columnInfo; } }
    }

    /// <summary>
    /// A new ColumnInfo instance is created on the first update for each table. The handler
    /// will read the columns for the table and store in a List<ColumnInfo>. These records are 
    /// used for pre-validating parameters and values provided to table updates.
    /// </summary>
    class ColumnInfo
    {
        string _dbName, _schemaName, _tableName, _columnName;
        int _columnId;
        bool _isNullable, _hasDefault;

        public ColumnInfo(DataRow row)
        {
            _columnId = Convert.ToInt32(row[0]);
            _columnName = row[4].ToString();
            _isNullable = Convert.ToBoolean(row[5]);
            _hasDefault = Convert.ToBoolean(row[6]);
        }

        public int ColumnId        { get { return _columnId; } }
        public string DatabaseName { get { return _dbName; } }
        public string SchemaName   { get { return _schemaName; } }
        public string TableName    { get { return _tableName; } }
        public string ColumnName   { get { return _columnName; } }
        public bool IsNullable     { get { return _isNullable; } }
        public bool HasDefault     { get { return _hasDefault; } }
    }
}
