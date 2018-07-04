using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeServer.Core
{
    public interface IData
    {
        bool Connected { get; }
        void Connect(string connectionString);
    }
}
