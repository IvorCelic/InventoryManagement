using System.Runtime.Versioning;

namespace ConsoleApp2
{
    internal class Utility
    {
        public static int LoadNumberRange(string message, string error, int start, int end)
        {
            int input;

            while (true)
            {
                Console.Write(message);

                try
                {
                    input = int.Parse(Console.ReadLine());

                    if (input >= start && input <= end)
                    {
                        return input;
                    }
                    else
                    {
                        Console.WriteLine(error);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(error);
                }

            }
        }

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

        public static string LoadString(string message, string error)
        {
            string input;

            while (true)
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (input.Trim().Length == 0)
                {
                    Console.WriteLine(error);
                    continue;
                }

                return input;
            }
        }

        public static string ValidateString(string message, string error)
        {
            string input;

            while (true)
            {
                Console.WriteLine(message);

                try
                {
                    input = Console.ReadLine();

                    if (!string.IsNullOrEmpty(input) && input.Any(char.IsLetter))
                    {
                        return input;
                    }
                    else
                    {
                        Console.WriteLine(error);
                    }

                }
                catch (Exception exception)
                {
                    Console.WriteLine(error);
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

        public static float LoadFloat(string message)
        {

            while (true)
            {
                Console.WriteLine(message);

                try
                {
                    string inputString = Console.ReadLine();
                    float input = float.Parse(inputString);

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

        public static int ValidateId()
        {
            return 0;
        }


    }
}
