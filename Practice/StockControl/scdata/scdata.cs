using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using System.ServiceModel;

namespace StockControl.Core.Data
{
    public delegate void MessageEventHandler(string Message);

    public class StockControlDTO
    {
        Timer timer;
        string _constring = null;
        List<DatabaseConnection> _connections = new List<DatabaseConnection>();

        #region Singleton Initialisation

        private static readonly StockControlDTO instance = new StockControlDTO();

        private StockControlDTO()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _constring = appSettings["ConnectionString"];

            if (_constring == "") throw new Exception("Connection string not found in the application settings file");

            timer = new Timer(new TimerCallback(TimerFired));
            timer.Change(1000, 1000);
        }

        public static StockControlDTO Instance { get { return instance; } }

        #endregion

        private void TimerFired(object state)
        {
            lock (_connections)
            {
                DateTime now = DateTime.Now;

                // Create a temporary list for old connections
                List<DatabaseConnection> l = new List<DatabaseConnection>();

                // Add purge pending connections to the temp list
                foreach (DatabaseConnection c in _connections)
                {
                    if (c.PurgePending && c.CloseDT.AddMinutes(1) < now) l.Add(c);
                }

                // Purge the connections
                foreach (DatabaseConnection c in l)
                {
                    _connections.Remove(c);
                    Console.WriteLine("Connection purged");
                }
            }
        }

        public event MessageEventHandler MessageEvent;

        private DatabaseConnection GetConnection()
        {
            DatabaseConnection con = null;

            lock (_connections)
            {
                foreach (DatabaseConnection c in _connections)
                {
                    if (c.PurgePending)
                    {
                        c.Open();
                        con = c;
                        break;
                    }
                }
                if (con == null)
                {
                    con = new DatabaseConnection(new SqlConnection(_constring));
                    _connections.Add(con);
                }
            }

            return con;
        }

        #region Member Data

           
        public DataTable GetTypes()
        {
            string queryString = "SELECT TypeID, Definition, Name, Sequence FROM dbo.Type";

            DatabaseConnection con = GetConnection();

            SqlCommand command = new SqlCommand(queryString, con.Connection);
            SqlDataAdapter _da = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Types");
            _da.Fill(dt);

            con.Close();

            return dt;
        }

        public DataTable GetCodes()
        {
            string queryString = "SELECT CodeID, CodeType, Definition, Name, Description, Sequence FROM dbo.Code";

            DatabaseConnection con = GetConnection();

            SqlCommand command = new SqlCommand(queryString, con.Connection);
            SqlDataAdapter _da = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Codes");
            _da.Fill(dt);

            con.Close();

            return dt;
        }

        public DataTable GetParameters()
        {
            string queryString = "SELECT ParmID, ParmCode, Definition, Name, Description, Value, ValueCode, Sequence FROM dbo.Parm";

            DatabaseConnection con = GetConnection();

            SqlCommand command = new SqlCommand(queryString, con.Connection);
            SqlDataAdapter _da = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Parameters");
            _da.Fill(dt);

            con.Close();
            
            return dt;
        }

        #endregion

        #region Common Functions

        /*
        private SqlCommand BuildSqlCommand(string tableName, DataRow row, string pkName)
        {
            SqlCommand cmd = null;
            List<SqlParameter> plist = null;

            string sql = "";

            int pcount = 0;
            for (int x = 0; x < row.ItemArray.Length; x++)
            {
                if (row[x, DataRowVersion.Original].ToString() != row[x, DataRowVersion.Current].ToString())
                {
                    string parm = String.Format("@p{0}", pcount++);

                    if (sql == "")
                    {
                        sql = String.Format("update {0} set [{1}] = {2}", tableName, row.Table.Columns[x].ColumnName, parm);
                        plist = new List<SqlParameter>();
                    }
                    else
                    {
                        sql += String.Format(", [{0}] = {1}", row.Table.Columns[x].ColumnName, parm);
                    }

                    plist.Add(new SqlParameter(parm, row[x]));
                }
            }

            if (sql != "")
            {
                cmd = new SqlCommand();
                cmd.Connection = _con;
                string parm = String.Format("@p{0}", pcount);
                sql = String.Format("{0} where [{1}] = {2}", sql, pkName, parm);
                plist.Add(new SqlParameter(parm, row[pkName]));
                cmd.Parameters.AddRange(plist.ToArray());
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
            }

            return cmd;
        }
        */

        #endregion
    }

    class DatabaseConnection
    {
        bool _pp = false;
        DateTime _odt, _cdt;
        SqlConnection _con;

        public DatabaseConnection(SqlConnection Connection)
        {
            _odt = DateTime.Now;
            _pp = false;
            _con = Connection;
        }

        public SqlConnection Connection { get { return _con; } }
        public DateTime OpenDT { get { return _odt; } }
        public DateTime CloseDT { get { return _cdt; } }
        public bool PurgePending { get { return _pp; } }

        public void Open()
        {
            if (PurgePending)
            {
                _pp = false;
                _odt = DateTime.Now;
                if (_con.State != ConnectionState.Open) _con.Open();
            }
        }

        public void Close()
        {
            _cdt = DateTime.Now;
            _pp = true;
            if (_con.State != ConnectionState.Closed) _con.Close();
        }
    }
}
