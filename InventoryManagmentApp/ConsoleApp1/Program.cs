





using ConsoleApp1.Models;

namespace ConsoleApp1
{
    internal class Program
    {
        private List<Person> Persons;
        public Program()
        {
            Persons = new List<Person>();
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
                    Console.WriteLine("You choosed to work with location");
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

            person.Id = Utility.LoadInt(person.Id + " - Insert new Id: ");
            person.FirstName = Utility.ValidateString(person.FirstName + "  - first name: ");
            person.LastName = Utility.ValidateString(person.LastName + " - last name: ");
            person.Email = Utility.LoadString(person.Email + " - email: ");
            person.Password = Utility.LoadString(person.Password + " - password: ");

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
