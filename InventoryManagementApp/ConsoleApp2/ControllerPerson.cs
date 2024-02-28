using ConsoleApp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class ControllerPerson
    {
        public List<Person> Persons { get; }

        public ControllerPerson()
        {
            Persons = new List<Person>();
        }

        public void NavigationMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("    Persons Navigation menu");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1. List all persons");
            Console.WriteLine("2. Add new person");
            Console.WriteLine("3. Edit person");
            Console.WriteLine("4. Delete person");
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
                    ListAllPersons();
                    NavigationMenu();
                    break;
                case 2:
                    Console.WriteLine("");
                    AddNewPerson();
                    NavigationMenu();
                    break;
                case 3:
                    EditPerson();
                    NavigationMenu();
                    break;
                case 4:
                    DeletePerson(); 
                    NavigationMenu();
                    break;
                case 5:
                    Console.WriteLine("Going back to navigation menu.");
                    break;

            }
        }

        private void DeletePerson()
        {
            if (Persons.Count > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("-------------");
                Console.WriteLine("delete person");
                Console.WriteLine("-------------");

                ListAllPersons();
                Console.WriteLine("");

                int index = Utility.LoadNumberRange("Choose person to delete: ", "Choose correct!", 1, Persons.Count);
                Persons.RemoveAt(index - 1);

                Console.WriteLine("");
                Console.WriteLine("Person successfully deleted.");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Currently there are no persons to delete.");
            }
        }

        private void EditPerson()
        {
            if (Persons.Count > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("-----------");
                Console.WriteLine("edit person");
                Console.WriteLine("-----------");

                ListAllPersons();
                Console.WriteLine("");

                int input = Utility.LoadNumberRange("Choose person to edit: ", "Choose correct!", 1, Persons.Count);
                var person = Persons[input - 1];

                person.Id = Utility.ValidateId($"Current ID: {person.Id}" + " | New ID: ", Persons, p => p.Id, person.Id, "Input must be whole positive number!");
                person.FirstName = Utility.LoadString($"Current first name: {person.FirstName}" + " | New first name: ", "Input required!");
                person.LastName = Utility.LoadString($"Current lasts name: {person.LastName}" + " | New last name: ", "Input required!");
                person.Email = Utility.LoadString($"Current email: {person.Email}" + " | New email: ", "Input required!");
                person.Password = Utility.LoadString($"Current password: {person.Password}" + " | New password: ", "Input required!");

                Console.WriteLine("");
                Console.WriteLine("Person edited successfully.");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Currently there are no persons to edit.");
            }

        }

        private void AddNewPerson()
        {
            Console.WriteLine("");
            Console.WriteLine("----------");
            Console.WriteLine("Add person");
            Console.WriteLine("----------");

            var person = new Person();

            person.Id = Utility.ValidateId("Add person's ID: ", Persons, p => p.Id, person.Id, "Input must be whole positive number!");
            person.FirstName = Utility.LoadString("Add person's first name: ", "Input required!");
            person.LastName = Utility.LoadString("add person's last name: ", "Input required!");
            person.Email = Utility.LoadString("Add person's email: ", "Input required!");
            person.Password = Utility.LoadString("Add person's password: ", "Input required!");

            Console.WriteLine("");
            Console.WriteLine("Person added successfully.");

            Persons.Add(person);
        }

        private void ListAllPersons()
        {
            Console.WriteLine("--------");
            Console.WriteLine("Products");
            Console.WriteLine("--------");

            var number = 0;
            Persons.ForEach(product =>
            {
                Console.WriteLine(++number + ". " + product);
            });
        }
    }
}
