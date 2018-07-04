using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<TaskData> action = (str) => Console.WriteLine("Task={0}, str={1}, Thread={2}", Task.CurrentId, str, Thread.CurrentThread.ManagedThreadId);

            Task<TaskData> wakeUp1 = (DoWorkAsync(2000, "Waking up 1 in 2s"));
            wakeUp1.ContinueWith(task => { WriteMessage(task.Result.Message); });

            Task<TaskData> wakeUp2 = DoWorkAsync(1000, "Waking up 2 in 1s");
            wakeUp2.ContinueWith(task => { WriteMessage(task.Result.Message); });

            Task<TaskData> wakeUp3 = DoWorkAsync(10000, "Waking up 3 in 10s");
            wakeUp3.ContinueWith(task => { WriteMessage(task.Result.Message); });

            Task<TaskData> wakeUp4 = DoWorkAsync(5000, "Waking up 5 in 5s");
            wakeUp4.ContinueWith(task => { WriteMessage(task.Result.Message); });

            Console.WriteLine("Waiting");

            //Task.WaitAll(wakeUp1, wakeUp2, wakeUp3, wakeUp4);
            Console.WriteLine("Press a key");
            Console.ReadLine();
        }

        static void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }

        static Task<TaskData> DoWorkAsync(int milliseconds, string name)
        {

            //error appears below on word Run
            return Task<TaskData>.Factory.StartNew(() =>
            {
                Console.WriteLine("[{0}] Starting:  {1}", DateTime.Now, name);
                Thread.Sleep(milliseconds);
                return new TaskData() { Message = String.Format("[{0}] I'm awake!!!: {1}", DateTime.Now, name) };
            });
        }
    }

    class TaskData
    {
        public string Message { get; set; }
    }
}
