//Base program
using System;
using System.Data;
using System.Threading;

namespace Ex03
{
    class Program
    {
        private static string x = "";
        static object _Lock = new object();
        enum ProgramState
        {
            WriterTurn,
            ReaderTurn,
            Stopped
        }

        static ProgramState state = ProgramState.WriterTurn;
        static void ThReadX(object i)
        {
            while(true){
                lock (_Lock){
                    while (state != ProgramState.ReaderTurn &&
                            state != ProgramState.Stopped)
                    {
                        Monitor.Wait(_Lock);
                    }

                    if (state == ProgramState.Stopped)
                    {
                        Console.WriteLine("Thread {0} exit", i);
                        break;
                    }
                    
                    Console.WriteLine("Thread-{0} : X = {1}", i, x);
                    state = ProgramState.WriterTurn;
                    
                    Monitor.PulseAll(_Lock);
                }
            }
        }

        static void ThWriteX()
        {
            string xx = "";
            while(true){
                lock (_Lock){
                    while(state != ProgramState.WriterTurn)
                    {
                        Monitor.Wait(_Lock);
                    }
                    
                    Console.Write("Input: ");
                    xx = Console.ReadLine();
                    if (xx == "exit")
                    {
                        state = ProgramState.Stopped;

                        // ปลุก Reader เพื่อให้เห็นว่า state เปลี่ยนเป็น Stopped
                        Monitor.PulseAll(_Lock);
                        break;
                    }
                    x = xx;
                    state = ProgramState.ReaderTurn;
                    
                    Monitor.PulseAll(_Lock);
                }
            }
        }

        static void Main()
        {
            Thread A = new Thread(ThReadX);
            Thread B = new Thread(ThWriteX);

            A.Start(1);
            B.Start();

            A.Join();
            B.Join();
        }
    }
}