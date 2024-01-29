using ConsoleApp1.Models;
using System;

namespace ConsoleApp1
{
    internal class Program
    {
        private List<Person> Persons;
        private List<Location> Locations;
        private List<Product> Products;

        public Program()
        {
            Persons = new List<Person>();
            Locations = new List<Location>();
            Products = new List<Product>();

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
            switch (Utility.LoadInt("Enter your choice: "))
            {
                case 1:
                    Console.WriteLine("You choosed to work with persons");
                    NavigationMenuPersons();
                    break;
                case 2:
                    Console.WriteLine("You choosed to work with locations");
                    break;
                case 3:
                    Console.WriteLine("You choosed to work with products");
                    NavigationMenuProducts();
                    break;
                case 4:
                    Console.WriteLine("You choosed to work with products in locations");
                    break;
                case 5:
                    Console.WriteLine("You are exiting program");
                    break;
                default:
                    Console.WriteLine("Invalid number!");
                    NavigationMenu();
                    break;
            }

        }

        private void NavigationMenuProducts()
        {
            Console.WriteLine("");
            Console.WriteLine("----------------------");
            Console.WriteLine("Products Navigation bar");
            Console.WriteLine("----------------------");
            Console.WriteLine("1. List all products");
            Console.WriteLine("2. Add new product");
            Console.WriteLine("3. Edit product");
            Console.WriteLine("4. Delete product");
            Console.WriteLine("5. Go back to navigation menu");
            Console.WriteLine("--------------------------------");

            ChooseNumberNavigationMenuProducts();

        }

        private void ChooseNumberNavigationMenuProducts()
        {
            switch (Utility.LoadInt("Enter your choice: "))
            {
                case 1:
                    Console.WriteLine("");
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Listing all products");
                    Console.WriteLine("-------------------");
                    ShowAllProducts();
                    NavigationMenuProducts();
                    break;
                case 2:
                    Console.WriteLine("");
                    Console.WriteLine("---------------");
                    Console.WriteLine("Add new product:");
                    Console.WriteLine("---------------");
                    AddProduct();
                    break;
                case 3:
                    Console.WriteLine("");
                    Console.WriteLine("------------");
                    Console.WriteLine("Edit product:");
                    Console.WriteLine("------------");
                    EditProduct();
                    break;
                case 4:
                    Console.WriteLine("");
                    Console.WriteLine("--------------");
                    Console.WriteLine("Delete product:");
                    Console.WriteLine("--------------");
                    DeleteProduct();
                    break;
                case 5:
                    Console.WriteLine("Going back to navigation menu");
                    NavigationMenu();
                    break;
                default:
                    Console.WriteLine("Invalid number!");
                    NavigationMenu();
                    break;
            }

        }

        private void DeleteProduct()
        {
            int productIndex = ValidateProductSelection("delete");
            Console.WriteLine("");

            if (productIndex != -1)
            {
                var product = Products[productIndex];

                Products.RemoveAt(productIndex);

                Console.WriteLine("");
                Console.WriteLine($"{product} with ID {product.Id} deleted successfuly.");

            }
            NavigationMenuProducts();

        }

        private void EditProduct()
        {
            int productIndex = ValidateProductSelection("edit");
            Console.WriteLine("");

            if (productIndex != -1)
            {
                var product = Products[productIndex];

                var personIndex = ValidatePersonSelection("edit");

                if (personIndex != -1)
                {
                    Person person = Persons[personIndex];

                    product.Id = Utility.LoadInt("Current: " + product.Id + " | New Id: ");
                    product.Name = Utility.LoadString("Current: " + product.Name + " | New name: ");
                    product.Description = Utility.LoadString("Current: " + product.Description + " | New description: ");
                    product.IsUnitary = Utility.LoadBool("Current: " + product.IsUnitary + " | Is product unitary or no?: ");
                    product.Person = person;
                }

                Console.WriteLine($"{product} with ID {product.Id} edited successfully");
            }

            NavigationMenuProducts();

        }

        private void AddProduct()
        {
            if (Persons.Count == 0)
            {
                Console.WriteLine("You can't add product because there are no persons available.");
                NavigationMenuProducts();
                return;
            }

            var personIndex = ValidatePersonSelection("add");
            Console.WriteLine("");

            if (personIndex != -1)
            {
                Person person = Persons[personIndex];

                Products.Add(new Product
                {
                    Id = Utility.LoadInt("Add product ID: "),
                    Name = Utility.LoadString("Add product name: "),
                    Description = Utility.LoadString("Add product description: "),
                    IsUnitary = Utility.LoadBool("Please decide if product is unitary or no: "),
                    Person = person
                });

                Console.WriteLine("Product added successfully.");
            }

            NavigationMenuProducts();

        }



        private void ShowAllProducts()
        {
            var i = 0;
            Products.ForEach(product =>
            {
                Console.WriteLine(++i + ". " + product);
            });

        }

        private int ValidateProductSelection(string type)
        {
            if (Products.Count == 0)
            {
                Console.WriteLine($"You can't {type} product, because there isn't one.");
                return -1;
            }

            while (true)
            {
                ShowAllProducts();
                int index = Utility.LoadInt($"Choose product you want to {type}: ") - 1;

                if (index >= 0 && index < Persons.Count)
                {
                    return index;
                }
                else
                {
                    Console.WriteLine("Invalid product ID. Please choose an existing ID: ");
                }

            }

        }

        private void NavigationMenuPersons()
        {
            Console.WriteLine("");
            Console.WriteLine("----------------------");
            Console.WriteLine("Persons Navigation bar");
            Console.WriteLine("----------------------");
            Console.WriteLine("1. List all persons");
            Console.WriteLine("2. Add new person");
            Console.WriteLine("3. Edit person");
            Console.WriteLine("4. Delete person");
            Console.WriteLine("5. Go back to navigation menu");
            Console.WriteLine("--------------------------------");

            ChooseNumberNavigationMenuPersons();

        }

        private void ChooseNumberNavigationMenuPersons()
        {
            switch (Utility.LoadInt("Enter your choice: "))
            {
                case 1:
                    Console.WriteLine("");
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Listing all persons");
                    Console.WriteLine("-------------------");
                    ShowAllPersons();
                    NavigationMenuPersons();
                    break;
                case 2:
                    Console.WriteLine("");
                    Console.WriteLine("---------------");
                    Console.WriteLine("Add new person:");
                    Console.WriteLine("---------------");
                    AddPerson();
                    break;
                case 3:
                    Console.WriteLine("");
                    Console.WriteLine("------------");
                    Console.WriteLine("Edit person:");
                    Console.WriteLine("------------");
                    EditPerson();
                    break;
                case 4:
                    Console.WriteLine("");
                    Console.WriteLine("--------------");
                    Console.WriteLine("Delete person:");
                    Console.WriteLine("--------------");
                    DeletePerson();
                    break;
                case 5:
                    Console.WriteLine("Going back to navigation menu");
                    NavigationMenu();
                    break;
                default:
                    Console.WriteLine("Invalid number!");
                    NavigationMenu();
                    break;
            }

        }

        private void DeletePerson()
        {
            int input = ValidatePersonSelection("delete");

            if (input >= 0)
            {
                var person = Persons[input];

                Persons.RemoveAt(input);

                Console.WriteLine("");
                Console.WriteLine($"{person} with ID {person.Id} deleted successfuly.");
            }

            NavigationMenuPersons();

        }

        private void EditPerson()
        {
            int personIndex = ValidatePersonSelection("edit");

            if (personIndex != -1)
            {
                var person = Persons[personIndex];

                person.Id = Utility.LoadInt("Current: " + person.Id + " | New Id: ");
                person.FirstName = Utility.ValidateString("Current: " + person.FirstName + " | New first name: ");
                person.LastName = Utility.ValidateString("Current: " + person.LastName + " | New last name: ");
                person.Email = Utility.LoadString("Current: " + person.Email + " | New email: ");
                person.Password = Utility.LoadString("Current: " + person.Password + " | New password: ");

                Console.WriteLine($"{person} with ID {person.Id} edited successfully.");
            }

            NavigationMenuPersons();

        }

        private void AddPerson()
        {
            Persons.Add(new Person
            {
                Id = Utility.LoadInt("Add person ID: "),
                FirstName = Utility.ValidateString("Add person name: "),
                LastName = Utility.ValidateString("Add person last name: "),
                Email = Utility.LoadString("Add person email: "),
                Password = Utility.LoadString("Add person password: ")
            });

            Console.WriteLine("Person added successfully.");

            NavigationMenuPersons();

        }

        private void ShowAllPersons()
        {
            var i = 0;
            Persons.ForEach(person =>
            {
                Console.WriteLine(++i + ". " + person);
            });

        }

        private int ValidatePersonSelection(string type)
        {
            if (Persons.Count == 0)
            {
                Console.WriteLine($"You can't {type} person, because there isn't one.");
                return -1;
            }

            while (true)
            {
                ShowAllPersons();
                int index = Utility.LoadInt($"Choose person you want to {type}: ") - 1;

                if (index >= 0 && index < Persons.Count)
                {
                    return index;
                }
                else
                {
                    Console.WriteLine("Invalid person ID. Please choose an existing ID: ");
                }

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
