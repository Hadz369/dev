using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IG.ThirdParty
{
    public class TP_DataHandler
    {
        SqlConnection _con;
        bool _connected = false;

        public TP_DataHandler(string constr) 
        {
            _con = new SqlConnection(constr);
            Start();
        }

        private void Start()
        {
            if (_con != null && _con.State != ConnectionState.Open)
            {
                _con.Open();
                _connected = true;
            }
        }

        public bool Connected { get { return _connected; } }

        public string GetSiteName()
        {
            string name = "<Unknown>";
            
            SqlCommand cmd = new SqlCommand("Select top 1 VenueName from obj.Venue", _con);
            object o = cmd.ExecuteScalar();

            if (o != null) name = o.ToString();

            return name;
        }
    }
}
