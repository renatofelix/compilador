using System;

namespace Comp_2018_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\Renato\Documents\Visual Studio 2017\Projects\Comp_2018_1\Comp_2018_1\text_file.txt");
            char[] text_Char = text.ToCharArray();
            //System.Console.WriteLine(text);
            for(int i = 0; i< text_Char.Length;i++)
            {
                System.Console.Write(text_Char[i]);
            }
            

            Console.ReadKey(true);
        }
    }
}
