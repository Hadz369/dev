using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace HS
{
    [CollectionDataContract(Namespace = "", Name = "PropertyBag", ItemName = "Property")]
    public class AuthList : List<string>
    {
        public AuthList() { }

        public AuthList(IEnumerable<string> rights)
        {
            foreach (string a in rights)
            {
                if (!this.Contains(a, new AuthEqualityComparer()))
                {
                    this.Add(a);
                }
            }
        }
    }

    class AuthEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string s1, string s2)
        {
            if (String.Compare(s1, s2, true) == 0)
                return true;
            else
                return false;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
