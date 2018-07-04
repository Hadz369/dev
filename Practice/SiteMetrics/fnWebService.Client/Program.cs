using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fnWebService.ThirdParty;

namespace fnWebService.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1 webservice = new Service1();
            string srt;

            for (int x = 0; x < 10; x++)
            {
                srt = webservice.simpleMethod("David Hadley");
                Console.WriteLine(srt);
                Console.WriteLine(webservice.anotherSimpleMethod(4, x));
            }

            Console.ReadLine();
        }
    }
}
