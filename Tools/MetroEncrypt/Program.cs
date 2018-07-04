using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using metropolis.encryption.cryptography;

namespace MetroEncrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            string hash = "";
            Hasher ec = new Hasher();
            hash = ec.ComputeHashSHA1(args[0], args[1]);

            Console.WriteLine(hash);
        }
    }
}
