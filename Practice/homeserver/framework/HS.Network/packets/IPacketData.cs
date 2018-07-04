using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS.Network
{
    public interface IPacketData
    {
        void Initialise(byte[] data);
    }
}
