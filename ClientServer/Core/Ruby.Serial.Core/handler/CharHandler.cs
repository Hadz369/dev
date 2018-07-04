using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ruby.Serial
{
    class CharHandler : IPortBuffer
    {
        public int MinPacketLen
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Length
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasData
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasChanges
        {
            get { throw new NotImplementedException(); }
        }

        public void Add(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public string GetPacket()
        {
            throw new NotImplementedException();
        }
    }
}
