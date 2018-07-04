using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfSearchControls
{
    public class StreetType : iSearchItem
    {
        int _id;
        string _value, _abbrev;

        public StreetType(int id, string value, string abbreviation)
        {
            _id = id;
            _value = value;
            _abbrev = abbreviation;
        }

        public int Id { get { return _id; } }
        public string Value { get { return _value; } }
        public string Abbreviation { get { return _abbrev; } }
        public string SearchString { get { return String.Format("{0}, {1}", _value, _abbrev); } }

        public override string ToString()
        {
            return _value;
        }
    }
}
