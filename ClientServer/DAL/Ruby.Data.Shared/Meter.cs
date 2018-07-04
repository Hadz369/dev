using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ruby.Data
{
    public class Meter
    {
        int _id, _value;
        string _name;
        bool _isChanged = true;

        public Meter(int id, string name, int value)
        {
            _id = id;
            _name = name;
            _value = value;
        }

        public int MeterId { get { return _id; } }
        public string Name { get { return _name; } }
        public int Value { get { return _value; } }
        public bool IsChanged { get { return _isChanged; } set { _isChanged = value; } }
    }
}
