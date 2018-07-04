using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Metro.Service
{
    [DataContract(Namespace = "")]
    public class Meter
    {
        public Meter(int id, string name, int value)
        {
            MeterId = id;
            Name = name;
            Value = value;
        }

        [DataMember]
        public int MeterId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Value { get; set; }
    }
}
