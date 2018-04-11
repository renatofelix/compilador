using Newtonsoft.Json.Linq;
using System;

namespace Comp_2018_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexico lex = new Lexico();
            lex.MachineStart();

            lex.ShowTable();
            Console.ReadKey(true);
        }
    }
}
