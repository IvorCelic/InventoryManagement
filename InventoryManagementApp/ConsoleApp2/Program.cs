

namespace ConsoleApp2
{
    internal class Program
    {
        public Program()
        {
            HelloMessage();
            NavigationMenu();
        }

        private void NavigationMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("--------------");
            Console.WriteLine("Navigation bar");
            Console.WriteLine("--------------");
            Console.WriteLine("1. Work with persons");
            Console.WriteLine("2. Work with locations");
            Console.WriteLine("3. Work with products");
            Console.WriteLine("4. Put products in locations");
            Console.WriteLine("5. Exit the application");
            Console.WriteLine("--------------------------------");

            ChooseNumberNavigationMenu();
        }

        private void ChooseNumberNavigationMenu()
        {

        }

        private void HelloMessage()
        {
            Console.WriteLine(" ______________________________");
            Console.WriteLine("|                              |");
            Console.WriteLine("|  INVENTORY MANAGMENT APP v1  |");
            Console.WriteLine("|______________________________|");

        }

        static void Main()
        {
            new Program();
        }
    }
}
