using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestServer
{
    public class Service : IService
    {
        public string EchoWithGet(string s)
        {
            return "You said " + s;
        }

        public string EchoWithPost(string s)
        {
            return "You said " + s;
        }
    }
}