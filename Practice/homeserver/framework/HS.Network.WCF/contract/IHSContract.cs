using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Reflection;

namespace HS.Network
{
    [ServiceKnownType("GetKnownTypes", typeof(KnownTypeHelper))]
    [ServiceContract()]
    public interface IHSContract
    {
        [OperationContract]
        bool KeepAlive();

        [OperationContract]
        Response ProcessRequest(Request request);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginRequestAsync(Request request, AsyncCallback callback, object asyncState);

        Response EndRequestAsync(IAsyncResult result);
    }

    static class KnownTypeHelper
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
                knownTypes.Add(typeof(Request));
                knownTypes.Add(typeof(Response));
                knownTypes.Add(typeof(FaultData));
                knownTypes.Add(typeof(System.Data.DataTable));
                knownTypes.Add(typeof(System.DBNull));
                knownTypes.Add(typeof(Property));
                knownTypes.Add(typeof(PropertyBag));
                knownTypes.Add(typeof(PropertyBagCollection));
                knownTypes.Add(typeof(Code));
                knownTypes.Add(typeof(CodeCollection));
                knownTypes.Add(typeof(Session));
                knownTypes.Add(typeof(SessionCollection));
            }

            return knownTypes;
        }
    }
}