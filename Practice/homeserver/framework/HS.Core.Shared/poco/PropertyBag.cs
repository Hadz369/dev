using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace HS
{
    [DataContract(Namespace = "")]
    public class Property
    {
        public Property() { }

        public Property(string name, object value)
        {
            Name = name;
            
            if (value.GetType() == typeof(System.DBNull)) Value = null;
            else Value = value;
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
                throw new InvalidPropertyException(String.Format("Error converting property {0} to {1}", Name, typeof(T).ToString()));
            }
        }
    }

    [CollectionDataContract(Namespace = "", Name = "PropertyBag", ItemName = "Property")]
    public class PropertyBag : List<Property> 
    {
        public PropertyBag() { }

        public PropertyBag(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public void Add(string name, object value)
        {
            Property p = new Property(name, value);
            this.Add(p);
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

        public string ToNameValueString()
        {
            string str = "";

            foreach (Property p in this)
            {
                str = String.Concat(str, (str == "" ? "" : ", "), p.Name, "=\"", p.Value.ToString(), "\"");  
            }

            return str;
        }
    }

    [CollectionDataContract(Namespace = "", Name = "PropertyBagCollection", ItemName = "PropertyBag")]
    public class PropertyBagCollection : List<PropertyBag>
    {
        public PropertyBagCollection() { }

        public PropertyBag GetPropertyBag(string name)
        {
            foreach (PropertyBag pb in this)
            {
                if (pb.Name == name)
                {
                    return pb;
                }
            }

            return null;
        }
    }

    public class InvalidPropertyException : Exception
    {
        public InvalidPropertyException(string message) : base(message) { }
    }
}
