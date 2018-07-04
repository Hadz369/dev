using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfSearchControls
{
    public class SearchItem : iSearchItem
    {
        int _id;
        string _value;

        public SearchItem(int id, string value)
        {
            _id = id;
            _value = value;
        }

        public int Id { get { return _id; } }
        public string Value { get { return _value; } }
        public string SearchString { get { return _value; } }

        public override string ToString()
        {
            return _value;
        }
    }
}
