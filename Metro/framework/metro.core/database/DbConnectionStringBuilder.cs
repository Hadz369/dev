using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metro
{
    public  class DbConnectionStringBuilder
    {
         string _server = "", _user = "", _pwd = "", _database = "";
         bool _usetrusted = false, _persist = false;

        //Properties
        public  bool UseTrusted { set { _usetrusted = value; } }

        public  string Server
        {
            set
            {
                string[] _parts = value.Split('\\');

                switch (_parts[0])
                {
                    case "(local)":
                    case "localhost":
                    case ".":
                        _parts[0] = "localhost";
                        break;
                    default:
                        break;
                }
                if (_parts.Length > 1)
                {
                    _server = String.Format("{0}\\{1}", _parts[0], _parts[1]);
                }
                else
                {
                    _server = _parts[0];
                }
            }
        }

        public  string UserName { set { _user = value; } }
        public  string Password { set { _pwd = value; } }
        public  string Database { set { _database = value; } }
        public  bool PersistSecurity { set { _persist = value; } }

        //Methods
        public  string GetConnectionString()
        {
            String constr = "";

            if (!_usetrusted && _user == "") return null;
            else
            {
                constr += "Data Source=" + _server + ";";

                //Add the database name
                if (_database != "") constr += "Initial Catalog=" + _database + ";";

                //Windows or SQL Server authentication
                if (_usetrusted) constr += "Integrated Security=SSPI;";
                else constr += "User Id=" + _user + ";Password=" + _pwd + ";";

                if (_persist) constr += "Persist Security Info=True;";
            }

            return constr;
        }
    }
}
