using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Reflection;

namespace Metro.Service
{
    [ServiceKnownType("GetKnownTypes", typeof(SystemTypeHelper))]
    [ServiceContract()]
    public interface ISystemServiceContract : IMetroContract
    {
    }

    static class SystemTypeHelper
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            List<Type> knownTypes = new List<Type>();

            foreach (Type t in CoreKnownTypes.GetTypes())
                knownTypes.Add(t);

            knownTypes.Add(typeof(Metro.Service.MemberInfo));
            knownTypes.Add(typeof(Metro.Service.Meter));
            knownTypes.Add(typeof(Metro.Service.MachineDetail));
            knownTypes.Add(typeof(Metro.Service.MachineDetailCollection));
            
            return knownTypes;
        }
    }

}
