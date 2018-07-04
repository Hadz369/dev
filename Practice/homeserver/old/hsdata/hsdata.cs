using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace HomeServer.Core
{
  	public sealed class MySql : IData
    {
        #region Singleton Initialisation

        private static readonly MySql instance = new MySql();

        private MySql() { }

        public static MySql Instance { get { return instance; } }

        #endregion

        MySqlConnection _con;
        string _constr;
        DataSet _dsLists = new DataSet();

        public void Connect(string ConnectionString)
        {
            if (_con == null)
            {
                _constr = ConnectionString;
                _con = new MySqlConnection(ConnectionString);
            }

            if (!Connected)
            {
                try
                {
                    _constr = ConnectionString;
                    _con = new MySqlConnection(_constr);
                    _con.Open();

                    LoadListData();
                }
                catch (MySqlException ex)
                {
                    string msg = String.Format("Error connecting to the database. Msg={0}", ex.Message);
                    throw new Exception(msg, ex);
                }
            }
            else
            {
                throw new Exception("" +
                    "The connection is already open. " +
                    "You must close the connection before calling Connect().");
            }
        }

        public bool Connected
        {
            get
            {
                if (_con == null || _con.State == ConnectionState.Broken || _con.State == ConnectionState.Closed)
                    return false;
                else
                    return true;
            }
        }

        void LoadListData()
        {
            MySqlCommand cmd;
            MySqlDataAdapter da;
            DataTable dt;

            cmd = new MySqlCommand("select * from type", _con);
            da = new MySqlDataAdapter(cmd);
            dt = new DataTable("type");
            da.Fill(dt);
            AddTableToDataSet(_dsLists, dt);

            cmd = new MySqlCommand("select * from code", _con);
            da = new MySqlDataAdapter(cmd);
            dt = new DataTable("code");
            da.Fill(dt);
            AddTableToDataSet(_dsLists, dt);

            cmd = new MySqlCommand("select * from enumeration", _con);
            da = new MySqlDataAdapter(cmd);
            dt = new DataTable("enum");
            da.Fill(dt);
            AddTableToDataSet(_dsLists, dt);

            cmd = new MySqlCommand("select * from parameter", _con);
            da = new MySqlDataAdapter(cmd);
            dt = new DataTable("parm");
            da.Fill(dt);
            AddTableToDataSet(_dsLists, dt);
        }

        void AddTableToDataSet(DataSet DS, DataTable DT)
        {
            if (DS.Tables.Contains(DT.TableName)) DS.Tables.Remove(DT.TableName);
            DS.Tables.Add(DT);
        }

        public DataTable GetList(string Name)
        {
            if (_dsLists.Tables.Contains(Name)) 
                return _dsLists.Tables[Name];
            else
                return null;
        }
	}
}
