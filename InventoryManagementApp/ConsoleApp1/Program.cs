using ConsoleApp1.Models;
using System;
using System.Reflection.Metadata;

namespace ConsoleApp1
{
    internal class Program
    {
        private List<Person> Persons;
        private List<Location> Locations;
        private List<Product> Products;
        private List<ProductLocation> ProductsLocation;

        public Program()
        {
            Persons = new List<Person>();
            Locations = new List<Location>();
            Products = new List<Product>();
            ProductsLocation = new List<ProductLocation>();

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
                    NavigationMenuLocations();
                    break;
                case 3:
                    Console.WriteLine("You choosed to work with products");
                    NavigationMenuProducts();
                    break;
                case 4:
                    Console.WriteLine("You choosed to work with products in locations");
                    NavigationMenuProductsLocation();
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

        private void NavigationMenuProductsLocation()
        {
            Console.WriteLine("");
            Console.WriteLine("----------------------");
            Console.WriteLine("Products in Locations Navigation bar");
            Console.WriteLine("----------------------");
            Console.WriteLine("1. List all products in locations");
            Console.WriteLine("2. Add product in location");
            Console.WriteLine("3. Edit product in location");
            Console.WriteLine("4. Delete product from location");
            Console.WriteLine("5. Go back to navigation menu");
            Console.WriteLine("--------------------------------");

            ChooseNumberNavigationMenuProductsLocations();

        }

        private void ChooseNumberNavigationMenuProductsLocations()
        {
            switch (Utility.LoadInt("Enter your choice: "))
            {
                case 1:
                    Console.WriteLine("");
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Listing all products on locations");
                    Console.WriteLine("-------------------");
                    ShowAllProductsLocations();
                    NavigationMenuProductsLocation();
                    break;
                case 2:
                    Console.WriteLine("");
                    Console.WriteLine("---------------");
                    Console.WriteLine("Add product to location:");
                    AddProductLocation();
                    Console.WriteLine("---------------");
                    break;
                case 3:
                    Console.WriteLine("");
                    Console.WriteLine("------------");
                    Console.WriteLine("Edit product in location:");
                    EditProductLocation();
                    Console.WriteLine("------------");
                    break;
                case 4:
                    Console.WriteLine("");
                    Console.WriteLine("--------------");
                    Console.WriteLine("Delete product from location:");
                    DeleteProductLocation();
                    Console.WriteLine("--------------");
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

        private void DeleteProductLocation()
        {
            int productLocationIndex = ValidateProductLocationSelection("delete");
            Console.WriteLine("");

            if (productLocationIndex != -1)
            {
                var productLocation = ProductsLocation[productLocationIndex];

                ProductsLocation.RemoveAt(productLocationIndex);

                Console.WriteLine("");
                Console.WriteLine($"{productLocation} with ID {productLocation.Id} deleted successfully.");
            }

            NavigationMenuProductsLocation();

        }

        private void EditProductLocation()
        {
            int productLocationIndex = ValidateProductLocationSelection("edit");
            Console.WriteLine("");

            if (productLocationIndex != -1)
            {
                var productLocation = ProductsLocation[productLocationIndex];

                var personIndex = ValidatePersonSelection("edit");

                if (personIndex != -1)
                {
                    Person person = Persons[personIndex];

                    var productIndex = ValidateProductSelection("edit");

                    if (productIndex != -1)
                    {
                        Product product = Products[productIndex];

                        var locationIndex = ValidateLocationSelection("edit");

                        if (locationIndex != -1)
                        {
                            Location location = Locations[locationIndex];

                            productLocation.Id = Utility.LoadInt("Current: " + productLocation.Id + " | New Id: ");
                            productLocation.Quantity = Utility.LoadInt("Current: " + productLocation.Quantity + " | New quantity: ");
                            productLocation.Price = Utility.LoadFloat("Current: " + productLocation.Price + " | New price: ");
                            productLocation.Product = product;
                            productLocation.Location = location;
                            productLocation.Person = person;
                        }
                    }
                }

                Console.WriteLine($"{productLocation} with ID {productLocation.Id} edited successfully.");

            }

            NavigationMenuProductsLocation();

        }

        private void AddProductLocation()
        {
            if (Products.Count == 0 && Locations.Count == 0 && Persons.Count == 0)
            {
                Console.WriteLine("You can't add product to location because there is no product, location or person.");
                NavigationMenuProductsLocation();
                return;
            }

            var personIndex = ValidatePersonSelection("add");
            Console.WriteLine("");
            var productIndex = ValidateProductSelection("add");
            Console.WriteLine("");
            var locationIndex = ValidateLocationSelection("add");
            Console.WriteLine("");

            if (personIndex != -1)
            {
                Person person = Persons[personIndex];

                if (productIndex != -1)
                {
                    Product product = Products[productIndex];

                    if (locationIndex != -1)
                    {
                        Location location = Locations[locationIndex];

                        ProductsLocation.Add(new ProductLocation
                        {
                            Id = Utility.LoadInt("Add ProductLocation ID: "),
                            Quantity = Utility.LoadInt("Add quantity: "),
                            Price = Utility.LoadFloat("Add price: "),
                            Product = product,
                            Location = location,
                            Person = person,
                        });

                        Console.WriteLine("Product to location added successfully.");
                    }
                }
            }

            NavigationMenuProductsLocation();

        }

        private void ShowAllProductsLocations()
        {
            var i = 0;
            ProductsLocation.ForEach(productLocation =>
            {
                Console.WriteLine(++i + ". " + productLocation);
            });

        }

        private int ValidateProductLocationSelection(string type)
        {
            if (Products.Count == 0 && Locations.Count == 0)
            {
                Console.WriteLine($"You can't {type} product in location, because there is no product & location currently added.");
                return -1;
            }
            else if (Products.Count == 0)
            {
                Console.WriteLine($"You can't {type} product in location, because there is no product currently added.");
                return -1;
            }
            else if (Locations.Count == 0)
            {
                Console.WriteLine($"You can't {type} product in location, because there is no location currently added.");
                return -1;
            }

            while (true)
            {
                ShowAllProductsLocations();
                int index = Utility.LoadInt($"Choose product in location you want to {type}: ") - 1;

                if (index >= 0 && index < ProductsLocation.Count)
                {
                    return index;
                }
                else
                {
                    Console.WriteLine("Invalid ProductLocation ID. Please choose an existing ID: ");
                }

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
                Console.WriteLine($"{product} with ID {product.Id} deleted successfully.");

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
                    product.IsUnique = Utility.LoadBool("Current: " + product.IsUnique + " | Is product unitary or no?: ");
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
                    IsUnique = Utility.LoadBool("Please decide if product is stackable or no: "),
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

                if (index >= 0 && index < Products.Count)
                {
                    return index;
                }
                else
                {
                    Console.WriteLine("Invalid product ID. Please choose an existing ID: ");
                }

            }

        }

        private void NavigationMenuLocations()
        {
            Console.WriteLine("");
            Console.WriteLine("----------------------");
            Console.WriteLine("Locations Navigation bar");
            Console.WriteLine("----------------------");
            Console.WriteLine("1. List all locations");
            Console.WriteLine("2. Add new location");
            Console.WriteLine("3. Edit location");
            Console.WriteLine("4. Delete location");
            Console.WriteLine("5. Go back to navigation menu");
            Console.WriteLine("--------------------------------");

            ChooseNumberNavigationMenuLocations();

        }

        private void ChooseNumberNavigationMenuLocations()
        {
            switch (Utility.LoadInt("Enter your choice: "))
            {
                case 1:
                    Console.WriteLine("");
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Listing all locations");
                    Console.WriteLine("-------------------");
                    ShowAllLocations();
                    NavigationMenuLocations();
                    break;
                case 2:
                    Console.WriteLine("");
                    Console.WriteLine("---------------");
                    Console.WriteLine("Add new location:");
                    Console.WriteLine("---------------");
                    AddLocation();
                    break;
                case 3:
                    Console.WriteLine("");
                    Console.WriteLine("------------");
                    Console.WriteLine("Edit location:");
                    Console.WriteLine("------------");
                    EditLocation();
                    break;
                case 4:
                    Console.WriteLine("");
                    Console.WriteLine("--------------");
                    Console.WriteLine("Delete location:");
                    Console.WriteLine("--------------");
                    DeleteLocation();
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

        private void DeleteLocation()
        {
            int locationIndex = ValidateLocationSelection("delete");
            Console.WriteLine("");

            if (locationIndex != -1)
            {
                var location = Locations[locationIndex];

                Locations.RemoveAt(locationIndex);

                Console.WriteLine("");
                Console.WriteLine($"{location} with ID {location.Id} deleted successfully.");
            }

            NavigationMenuLocations();

        }

        private void EditLocation()
        {
            int locationIndex = ValidateLocationSelection("edit");
            Console.WriteLine("");

            if (locationIndex != -1)
            {
                var location = Locations[locationIndex];

                location.Id = Utility.LoadInt("Current: " + location.Id + " | New Id: ");
                location.Name = Utility.LoadString("Current: " + location.Name + " | New name: ");
                location.Description = Utility.LoadString("Current: " + location.Description + " | New description: ");

                Console.WriteLine($"{location} with ID {location.Id} edited successfully");
            }

            NavigationMenuLocations();

        }

        private void AddLocation()
        {
            Locations.Add(new Location
            {
                Id = Utility.LoadInt("Add location ID: "),
                Name = Utility.LoadString("Add location name: "),
                Description = Utility.LoadString("Add location description: ")
            });

            Console.WriteLine("Location added successfully.");

            NavigationMenuLocations();
        }

        private void ShowAllLocations()
        {
            var i = 0;
            Locations.ForEach(location =>
            {
                Console.WriteLine(++i + ". " + location);
            });

        }

        private int ValidateLocationSelection(string type)
        {
            if (Locations.Count == 0)
            {
                Console.WriteLine($"You can't {type} location, because there isn't one.");
                return -1;
            }

            while (true)
            {
                ShowAllLocations();
                int index = Utility.LoadInt($"Choose location you want to {type}: ") - 1;

                if (index >= 0 && index < Locations.Count)
                {
                    return index;
                }
                else
                {
                    Console.WriteLine("Invalid location ID. Please choose an existing ID: ");
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

                bool isPersonOnProduct = Products.Any(product => product.Person == person);

                if (isPersonOnProduct)
                {
                    Console.WriteLine("");
                    Console.WriteLine($"Can't delete {person} because there are products associated with this person.");

                }
                else
                {
                    Persons.RemoveAt(input);

                    Console.WriteLine("");
                    Console.WriteLine($"{person} with ID {person.Id} deleted successfuly.");
                }

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
