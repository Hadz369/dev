using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Reflection;

namespace Metro
{
    [ServiceKnownType("GetKnownTypes", typeof(MetroTypeHelper))]
    [ServiceContract()]
    public interface IMetroContract
    {
        [OperationContract]
        Response ProcessRequest(Request request);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginRequestAsync(Request request, AsyncCallback callback, object asyncState);

        Response EndRequestAsync(IAsyncResult result);
    }

    static class MetroTypeHelper
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            return CoreKnownTypes.GetTypes();
        }
    }

    public static class CoreKnownTypes
    {
        static List<Type> knownTypes = new List<Type>();

        public static IEnumerable<Type> GetTypes()
        {
            if (knownTypes.Count == 0)
            {
                // Add any types to include here.
                knownTypes.Add(typeof(Metro.Request));
                knownTypes.Add(typeof(Metro.Response));
                knownTypes.Add(typeof(Metro.FaultData));
                knownTypes.Add(typeof(System.Data.DataTable));
                knownTypes.Add(typeof(System.DBNull));
                knownTypes.Add(typeof(Metro.Property));
                knownTypes.Add(typeof(Metro.PropertyBag));
                knownTypes.Add(typeof(Metro.PropertyBagCollection));
                knownTypes.Add(typeof(Metro.Code));
                knownTypes.Add(typeof(Metro.CodeCollection));
                knownTypes.Add(typeof(Metro.Session));
                knownTypes.Add(typeof(Metro.SessionCollection));
            }

            return knownTypes;
        }
    }
}