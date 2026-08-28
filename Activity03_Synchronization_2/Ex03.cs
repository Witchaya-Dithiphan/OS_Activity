//Base program
using System;
using System.Threading;

namespace Ex03
{
    class Program
    {
        private static string x = "";
        private static int exitflag = 0;

        static void ThReadX(object i)
        {
            while (exitflag == 0)
            {
                Console.WriteLine("Thread-{0} : X = {1}", i, x);
            }
            Console.WriteLine("Thread {0} exit", i);
        }

        static void ThWriteX()
        {
            string xx = "";
            while (exitflag == 0)
            {
                Console.WriteLine("Input: ");
                xx = Console.ReadLine();
                if (xx == "exit")
                    exitflag = 1;
                else
                    x = xx;
            }
        }

        static void Main()
        {
            Thread A = new Thread(ThReadX);
            Thread B = new Thread(ThWriteX);

            A.Start(1);
            B.Start();
        }
    }
}