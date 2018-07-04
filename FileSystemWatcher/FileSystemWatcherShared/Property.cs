using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ProtoBuf;

namespace FSW
{
    [DataContract(Namespace = "http://www.hadz.net/FSW")]
    public class Property
    {
        public Property() { }

        public Property(string name, object value)
        {
            Name = name;
            
            if (null == value || value.GetType() == typeof(System.DBNull)) 
                Value = null;
            else 
                Value = value;
        }

        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public object Value { get; set; }

        public T GetValue<T>()
        {
            try
            {
                return (T)Convert.ChangeType(Value, typeof(T));
            }
            catch
            {
                throw new ArgumentException(String.Format("Error converting property {0} to {1}", Name, typeof(T).ToString()));
            }
        }
    }

    [CollectionDataContract(Namespace = "http://www.hadz.net/FSW", Name = "PropertyBag", ItemName = "Property")]
    public class PropertyBag : List<Property>
    {
        public PropertyBag() { }

        public PropertyBag(string name)
        {
            Name = name;
        }

        [DataMember]
        public string Name { get; set; }

        public void Add(string name, object value)
        {
            Property p = new Property(name, value);
            this.Add(p);
        }

        public void Update(string name, object value)
        {
            Property p = GetProperty(name);
            if (p != null)
            {
                p.Value = value;
            }
        }

        public void Remove(string name)
        {
            Property p = GetProperty(name);
            if (p != null)
            {
                this.Remove(p);
            }
        }

        public Property GetProperty(string name)
        {
            foreach (Property p in this)
            {
                if (String.Compare(p.Name, name, true) == 0)
                {
                    return p;
                }
            }

            return null;
        }
    }
}
