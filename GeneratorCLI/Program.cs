using Generator;
using System;

namespace GeneratorCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            PseudoRandomSource s = new PseudoRandomSource();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(s.Next());
            }
        }
    }
}
