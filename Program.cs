using System;
using System.IO;

namespace Lndscaper
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = "./Test files";
            var lndFilter = "*.lnd";
            Console.WriteLine($"Currently in {Directory.GetCurrentDirectory()}");
            var lndFiles = Directory.EnumerateFiles(dir, lndFilter);
            foreach (var lndFileName in lndFiles)
            {
                Console.WriteLine(lndFileName);
            }
        }
    }
}
