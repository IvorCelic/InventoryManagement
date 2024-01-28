using ConsoleApp1.Models;

namespace ConsoleApp1
{
    internal class Program
    {
        private List<Person> Persons;
        private List<Location> Locations;
        public Program()
        {
            Persons = new List<Person>();
            Locations = new List<Location>();
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
            ShowAllLocations();

            Locations.RemoveAt(Utility.LoadInt("Choose location to delete: ") - 1);

            NavigationMenuLocations();

        }

        private void EditLocation()
        {
            ShowAllLocations();
            var location = Locations[Utility.LoadInt("Choose location to edit: ") - 1];

            location.Id = Utility.LoadInt("Current: " + location.Id + " | New Id: ");
            location.Name = Utility.LoadString("Current: " + location.Name + " | New name: ");
            location.Description = Utility.LoadString("Current: " + location.Description + " | New description: ");

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
            ShowAllPersons();
            Persons.RemoveAt(Utility.LoadInt("Choose person to delete: ") - 1);

            NavigationMenuPersons();

        }

        private void EditPerson()
        {
            ShowAllPersons();
            var person = Persons[Utility.LoadInt("Choose person to edit: ") - 1];

            person.Id = Utility.LoadInt("Current: " + person.Id + " | New Id: ");
            person.FirstName = Utility.ValidateString("Current: " + person.FirstName + " | New first name: ");
            person.LastName = Utility.ValidateString("Current: " + person.LastName + " | New last name: ");
            person.Email = Utility.LoadString("Current: " + person.Email + " | New email: ");
            person.Password = Utility.LoadString("Current: " + person.Password + " | New password: ");

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

            NavigationMenuPersons();

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
