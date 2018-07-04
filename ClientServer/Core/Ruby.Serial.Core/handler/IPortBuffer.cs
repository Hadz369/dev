using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ruby.Serial
{
    public interface IPortBuffer
    {
        int MinPacketLen { get; set; }

        int Length { get; }
        bool HasData { get; }
        bool HasChanges { get; }

        void Add(byte[] bytes);
        string GetPacket();
    }
}
