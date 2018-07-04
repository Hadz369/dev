using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ruby.Data
{
    public interface IDataService
    {
        void Start(string constring);
        void Stop();
    }
}
