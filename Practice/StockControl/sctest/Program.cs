using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using StockControl.Common;

namespace sctest
{
    class Program
    {
        static Client c;

        static void Main(string[] args)
        {
            c = new Client();

            Start();

            Console.WriteLine(c.WebHandler.ReverseString("Hello from the web handler"));

            while (true)
            {
                try
                {
                    DataTable dt = c.ClientHandler.GetTypes();
                    foreach (DataRow r in dt.Rows)
                    {
                        Console.WriteLine("{0}, {1}, {2}", r[0], r[1], r[2]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: Type={0}, Msg={1}", ex.GetType().ToString(), ex.Message);
                    Start();
                }

                Console.ReadLine();
            }
        }

        static void Start()
        {
            bool mhstopped = true, chstopped = true;

            while (mhstopped)
            {
                try
                {
                    c.StartMessageHandler();
                    c.MessageHandler.Subscribe();
                    mhstopped = false;
                }
                catch 
                {
                    Console.WriteLine("Error connecting to the message handler. Retrying in 5 seconds.");
                    Thread.Sleep(5000); 
                }

            }

            while (chstopped)
            {
                try
                {
                    c.StartClientHandler();
                    c.ClientHandler.Subscribe();
                    chstopped = false;

                    for (int x=0;x<5;x++)
                    {
                        c.ClientHandler.DoStuff();
                    }
                }
                catch 
                {
                    Console.WriteLine("Error connecting to the client handler. Retrying in 5 seconds."); 
                    Thread.Sleep(5000);
                }
            }


            c.StartWebHandler();
        }
    }
}
