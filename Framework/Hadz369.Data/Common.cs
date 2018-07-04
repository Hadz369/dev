using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Hadz369.Framework
{
    public enum DbEngine
    {
        SqlServer,
        MySql,
    }

    public class Connection
    {
        DateTime _created, _lastUsed;
        DbConnection _con;

        public Connection(DbConnection dbConnection)
        {
            _created = DateTime.Now;
            _con = dbConnection;
        }
    }

    public class DataManager
    {
        string _conString;
        List<DbConnection> _connections;

        public DataManager(string ConnectionString)
        {
            _conString = ConnectionString;
        }

        private System.Data.Common.DbConnection GetConnection()
        {
            System.Data.Common.DbConnection c = null;

            DbEngine d = DbEngine.MySql;

            switch (d)
            {
                case DbEngine.SqlServer:
                    c = new SqlConnection();
                    break;
                case DbEngine.MySql:
                    c = new MySqlConnection();
                    break;
            }

            return c;
        }

        public void ReleaseConnection(IDbConnection DbConnection)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar() { return null; }
        public void ExecuteNonQuery() { }

    }

    class TestMe : IDbCommand
    {
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public string CommandText
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CommandTimeout
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public CommandType CommandType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbConnection Connection
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbDataParameter CreateParameter()
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public IDataParameterCollection Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction Transaction
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
