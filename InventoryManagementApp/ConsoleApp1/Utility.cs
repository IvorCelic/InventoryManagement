using ConsoleApp1.Models;

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
                    string inputString = Console.ReadLine();
                    int input = int.Parse(inputString);

                    if (input > 0)
                    {
                        return input;
                    }
                    else
                    {
                        Console.WriteLine("Invalid number!");
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input!");
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

        public static string ValidateString(string message)
        {
            while (true)
            {
                try
                {
                    string input = LoadString(message);

                    if (!string.IsNullOrEmpty(input) && input.Any(char.IsLetter))
                    {
                        return input;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input!");
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input!");
                }
            }

        }

        public static bool LoadBool(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            while (true)
            {
                int input = LoadInt("Enter your choice (1 or 2): ");

                switch (input)
                {
                    case 1:
                        return true;
                    case 2:
                        return false;
                    default:
                        Console.WriteLine("You can choose 1 or 2!");
                        continue;
                }
            }

        }


    }
}
