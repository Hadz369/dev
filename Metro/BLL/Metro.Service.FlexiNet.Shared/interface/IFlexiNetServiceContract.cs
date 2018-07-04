using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Reflection;

namespace Metro.Service
{
    [ServiceKnownType("GetKnownTypes", typeof(FlexiNetTypeHelper))]
    [ServiceContract()]
    public interface IFlexiNetServiceContract : IMetroContract
    {
    }

    static class FlexiNetTypeHelper
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            List<Type> knownTypes = new List<Type>();

            foreach (Type t in CoreKnownTypes.GetTypes())
                knownTypes.Add(t);

            return knownTypes;
        }
    }

}
