//using ConsoleApp1.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleApp1
//{
//    internal class nekorišteno
//    {



//        private void NavigationMenuLocations()
//        {
//            Console.WriteLine("");
//            Console.WriteLine("----------------------");
//            Console.WriteLine("Locations Navigation bar");
//            Console.WriteLine("----------------------");
//            Console.WriteLine("1. List all locations");
//            Console.WriteLine("2. Add new location");
//            Console.WriteLine("3. Edit location");
//            Console.WriteLine("4. Delete location");
//            Console.WriteLine("5. Go back to navigation menu");
//            Console.WriteLine("--------------------------------");

//            ChooseNumberNavigationMenuLocations();

//        }

//        private void ChooseNumberNavigationMenuLocations()
//        {
//            switch (Utility.LoadInt("Enter your choice: "))
//            {
//                case 1:
//                    Console.WriteLine("");
//                    Console.WriteLine("-------------------");
//                    Console.WriteLine("Listing all locations");
//                    Console.WriteLine("-------------------");
//                    ShowAllLocations();
//                    NavigationMenuLocations();
//                    break;
//                case 2:
//                    Console.WriteLine("");
//                    Console.WriteLine("---------------");
//                    Console.WriteLine("Add new location:");
//                    Console.WriteLine("---------------");
//                    AddLocation();
//                    break;
//                case 3:
//                    Console.WriteLine("");
//                    Console.WriteLine("------------");
//                    Console.WriteLine("Edit location:");
//                    Console.WriteLine("------------");
//                    EditLocation();
//                    break;
//                case 4:
//                    Console.WriteLine("");
//                    Console.WriteLine("--------------");
//                    Console.WriteLine("Delete location:");
//                    Console.WriteLine("--------------");
//                    DeleteLocation();
//                    break;
//                case 5:
//                    Console.WriteLine("Going back to navigation menu");
//                    NavigationMenu();
//                    break;
//                default:
//                    Console.WriteLine("Invalid number!");
//                    NavigationMenu();
//                    break;
//            }

//        }

//        private void DeleteLocation()
//        {
//            ShowAllLocations();

//            Locations.RemoveAt(Utility.LoadInt("Choose location to delete: ") - 1);

//            NavigationMenuLocations();

//        }

//        private void EditLocation()
//        {
//            ShowAllLocations();
//            var location = Locations[Utility.LoadInt("Choose location to edit: ") - 1];

//            location.Id = Utility.LoadInt("Current: " + location.Id + " | New Id: ");
//            location.Name = Utility.LoadString("Current: " + location.Name + " | New name: ");
//            location.Description = Utility.LoadString("Current: " + location.Description + " | New description: ");

//            NavigationMenuLocations();

//        }

//        private void AddLocation()
//        {
//            Locations.Add(new Location
//            {
//                Id = Utility.LoadInt("Add location ID: "),
//                Name = Utility.LoadString("Add location name: "),
//                Description = Utility.LoadString("Add location description: ")
//            });

//            NavigationMenuLocations();
//        }

//        private void ShowAllLocations()
//        {
//            var i = 0;
//            Locations.ForEach(location =>
//            {
//                Console.WriteLine(++i + ". " + location);
//            });

//        }





//    }
//}
