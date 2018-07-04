using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Reflection;

namespace Metro.Service
{
    [ServiceKnownType("GetKnownTypes", typeof(GatewayHelper))]
    [ServiceContract()]

    public interface IGatewayServiceContract : IMetroServiceContract
    {
    }

    // This class has the method named GetKnownTypes that returns a generic IEnumerable.
    static class GatewayHelper
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            System.Collections.Generic.List<System.Type> knownTypes =
                new System.Collections.Generic.List<System.Type>();
            // Add any types to include here.
            knownTypes.Add(typeof(MemberInfo));
            return knownTypes;
        }
    }
}
