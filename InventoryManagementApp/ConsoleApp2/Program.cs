namespace ConsoleApp2
{
    internal class Program
    {
        public ControllerLocation ControllerLocation { get; }
        public ControllerProduct ControllerProduct { get; }

        public Program()
        {
            ControllerLocation = new ControllerLocation();
            ControllerProduct = new ControllerProduct();

            HelloMessage();
            NavigationMenu();
        }

        private void NavigationMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("        Navigation menu");
            Console.WriteLine("--------------------------------");
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
            Console.WriteLine("");

            switch (Utility.LoadNumberRange("Choose number from navigation menu: ", "Choose correct!", 1, 5))
            {
                case 1:
                    NavigationMenu();
                    break;
                case 2:
                    ControllerLocation.NavigationMenu();
                    NavigationMenu();
                    break;
                case 3:
                    ControllerProduct.NavigationMenu();
                    NavigationMenu();
                    break;
                case 4:
                    NavigationMenu();
                    break;
                case 5:
                    Console.WriteLine("You are exiting application. Goodbye.");
                    break;

            }
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
