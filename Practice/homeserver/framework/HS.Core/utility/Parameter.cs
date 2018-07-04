using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace HS
{
    /// <summary>
    /// Class used to read command line parameters into a dictionary
    /// </summary>
    public class ParameterReader
    {
        // This is used for key validation
        static Dictionary<string, ParameterKey> _validkeys = new Dictionary<string, ParameterKey>();

        ArrayList _invalid;
        string _delimiter = "/";

        public Dictionary<string, ParameterKey> ValidKeys { get { return _validkeys; } set { _validkeys = value; } }

        public string Delimiter
        {
            get
            {
                return _delimiter;
            }
            set
            {
                if (value == "/" || value == "-") _delimiter = value;
            }
        }

        public Dictionary<string, List<string>> ParseToDictionary(string[] args)
        {
            Dictionary<string, List<string>> parms = new Dictionary<string, List<string>>();

            if (args.Length > 0)
            {
                _invalid = new ArrayList();
                string key = "";

                for (int i = 0; i < args.Length; i++)
                {
                    string arg = args[i];

                    if (arg != _delimiter)
                    {
                        if (arg.StartsWith(_delimiter))
                        {
                            key = arg.Substring(1);
                            if (!parms.ContainsKey(key)) parms.Add(key, new List<string>());
                        }
                        else
                        {
                            if (key != "") parms[key].Add(arg);
                            else _invalid.Add(arg);
                        }
                    }
                }
            }

            return parms;
        }

        public string[] InvalidKeys
        {
            get
            {
                if (_invalid == null) return null;
                else return (string[])_invalid.ToArray(typeof(string));
            }
        }
    }

    public class ParameterKey
    {
        string _name, _desc;

        public ParameterKey(string name, string description)
        {
            _name = name;
            _desc = description;
        }

        public string Name { get { return _name; } }
        public string Description { get { return _desc; } }
    }
}
