using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Utility
    {
        public static int LoadInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                try
                {
                    return int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Incorrect input");
                }
            }
        }

        public static string LoadString(string message)
        {
            string input;

            while (true)
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (input.Trim().Length == 0)
                {
                    Console.WriteLine("Input is required");
                    continue;
                }

                return input;

            }
        }
    }
}
