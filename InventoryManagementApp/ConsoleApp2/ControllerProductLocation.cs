using ConsoleApp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class ControllerProductLocation
    {
        public List<ProductLocation> ProductsLocation { get; }

        private Program Program;

        public ControllerProductLocation (Program program) : this()
        {
            this.Program = program;
        }

        public ControllerProductLocation()
        {
            ProductsLocation = new List<ProductLocation>();
        }

        public void NavigationMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("    Products on locations Navigation menu");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("1. List all products in locations");
            Console.WriteLine("2. Add products to location");
            Console.WriteLine("3. Edit products in location");
            Console.WriteLine("4. Delete products from location");
            Console.WriteLine("5. Go back to navigation menu");
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
                    Console.WriteLine("");
                    NavigationMenu();
                    break;
                case 3:
                    NavigationMenu();
                    break;
                case 4:
                    NavigationMenu();
                    break;
                case 5:
                    Console.WriteLine("Going back to navigation menu.");
                    break;

            }
        }
    }
}
