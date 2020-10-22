using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleInstanceExample
{
    class Program : ISingleInstanceApp
    {
        static void Main(string[] args)
        {

            Program p = new Program();

            if (SingleInstanceApp.InitializeSingleInstanceApp(p))
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("App is running ...");
                }
            }

        }

        public void IncomingArgs(string[] args)
        {
            Console.WriteLine(string.Join(" ", args));
        }
    }
}
