using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CryptDiary.Data
{
    public class HashtagDictionary : Dictionary<Hashtag, List<DateTime>>
    {
        public HashtagDictionary()
        {
        }

        public HashtagDictionary(IDictionary<Hashtag, List<DateTime>> dictionary) : base(dictionary)
        {
        }

        public HashtagDictionary(IEqualityComparer<Hashtag> comparer) : base(comparer)
        {
        }

        public HashtagDictionary(int capacity) : base(capacity)
        {
        }

        public HashtagDictionary(IDictionary<Hashtag, List<DateTime>> dictionary, IEqualityComparer<Hashtag> comparer) : base(dictionary, comparer)
        {
        }

        public HashtagDictionary(int capacity, IEqualityComparer<Hashtag> comparer) : base(capacity, comparer)
        {
        }

        protected HashtagDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
