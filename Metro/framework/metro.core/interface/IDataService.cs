using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metro
{
    public interface IDataService
    {
        void Start(string constring);
        void Stop();
    }
}
