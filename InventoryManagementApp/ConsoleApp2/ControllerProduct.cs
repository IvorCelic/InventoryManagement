using ConsoleApp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class ControllerProduct
    {
        public List<Product> Products { get; }

        public ControllerProduct()
        {
            Products = new List<Product>();
        }

        public void NavigationMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("    Product Navigation menu");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("1. List all products");
            Console.WriteLine("2. Add new product");
            Console.WriteLine("3. Edit product");
            Console.WriteLine("4. Delete product");
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
                    ListAllProducts();
                    NavigationMenu();
                    break;
                case 2:
                    Console.WriteLine("");
                    AddNewProduct();
                    NavigationMenu();
                    break;
                case 3:
                    EditLocation();
                    NavigationMenu();
                    break;
                case 4:
                    DeleteProduct();
                    NavigationMenu();
                    break;
                case 5:
                    Console.WriteLine("You are exiting application. Goodbye.");
                    break;

            }
        }

        private void DeleteProduct()
        {
            throw new NotImplementedException();
        }

        private void EditLocation()
        {
            throw new NotImplementedException();
        }

        private void AddNewProduct()
        {
            Console.WriteLine("");
            Console.WriteLine("-----------");
            Console.WriteLine("Add product");
            Console.WriteLine("-----------");

            var product = new Product();

            product.Id = Utility.ValidateId();
            product.Name = Utility.LoadString("Add product name: ", "Input required!");

            Console.WriteLine("");
            Console.WriteLine("Location added successfully.");

            Products.Add(product);
        }

        private void ListAllProducts()
        {
            Console.WriteLine("--------");
            Console.WriteLine("Products");
            Console.WriteLine("--------");

            var number = 0;
            Products.ForEach(product =>
            {
                Console.WriteLine(++number + ". " + product);
            });
        }
    }
}
