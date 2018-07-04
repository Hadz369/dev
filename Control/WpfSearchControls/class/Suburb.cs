using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfSearchControls
{
    public class Suburb : iSearchItem
    {
        int _id;
        string _value, _postcode;

        public Suburb(int id, string value, string postcode)
        {
            _id = id;
            _value = value;
            _postcode = postcode;
        }

        public int Id { get { return _id; } }
        public string Value { get { return _value; } }
        public string PostCode { get { return _postcode; } }
        public string SearchString { get { return String.Format("{0}, {1}", _value, _postcode); } }

        public override string ToString()
        {
            return _value;
        }
    }
}
