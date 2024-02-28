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
                    EditProduct();
                    NavigationMenu();
                    break;
                case 4:
                    DeleteProduct();
                    NavigationMenu();
                    break;
                case 5:
                    Console.WriteLine("Going back to navigation menu.");
                    break;

            }
        }

        private void DeleteProduct()
        {
            if (Products.Count > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("--------------");
                Console.WriteLine("delete product");
                Console.WriteLine("--------------");

                ListAllProducts();
                Console.WriteLine("");

                int index = Utility.LoadNumberRange("Choose product to delete: ", "Choose correct!", 1, Products.Count);
                Products.RemoveAt(index - 1);

                Console.WriteLine("");
                Console.WriteLine("Product successfully deleted.");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Currently there are no products to delete.");
            }
        }

        private void EditProduct()
        {
            if (Products.Count > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("------------");
                Console.WriteLine("edit product");
                Console.WriteLine("------------");

                ListAllProducts();
                Console.WriteLine("");

                int input = Utility.LoadNumberRange("Choose product to edit: ", "Choose correct!", 1, Products.Count);
                var product = Products[input - 1];

                product.Id = Utility.ValidateId($"Current ID: {product.Id}" + " | New ID: ", Products, p => p.Id, product.Id, "Input must be whole positive number!");
                product.Name = Utility.LoadString($"Current name: {product.Name}" + " | New name: ", "Input required!");
                product.Description = Utility.LoadString($"Current description: {product.Description}" + " | New description: ", "Input required!");
                product.IsUnique = Utility.LoadBool($"Current uniqueness: {product.Name}" + " | New name: ", "Input required!");

                Console.WriteLine("");
                Console.WriteLine("Product edited successfully.");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Currently there are no products to edit.");
            }

        }

        private void AddNewProduct()
        {
            Console.WriteLine("");
            Console.WriteLine("-----------");
            Console.WriteLine("Add product");
            Console.WriteLine("-----------");

            var product = new Product();

            product.Id = Utility.ValidateId("Add product ID: ", Products, p => p.Id, product.Id, "Input must be whole positive number!");
            product.Name = Utility.LoadString("Add product name: ", "Input required!");
            product.Description = Utility.LoadString("add product description: ", "Input required!");
            product.IsUnique = Utility.LoadBool("Choose is product unique or no.", "Choose correct!");

            Console.WriteLine("");
            Console.WriteLine("Product added successfully.");

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
