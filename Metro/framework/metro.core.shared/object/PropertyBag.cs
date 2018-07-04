using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Metro
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

        public Property GetProperty(string name)
        {
            foreach (Property p in this)
            {
                if (p.Name == name)
                {
                    return p;
                }
            }
            
            return null;            
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
