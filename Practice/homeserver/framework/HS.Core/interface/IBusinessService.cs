using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS
{
    public interface IBusinessService
    {
        string Key { get; }
        bool IsRunning { get; }
        PropertyBag Properties { get; }

        void Start();
        void Stop();
    }
}
