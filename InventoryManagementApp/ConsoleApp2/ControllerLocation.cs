using ConsoleApp2.Models;

namespace ConsoleApp2
{
    internal class ControllerLocation
    {
        public List<Location> Locations { get; }

        public ControllerLocation() 
        {
            Locations = new List<Location>();
        }

        public void NavigationMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("    Location Navigation menu");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("1. List all locations");
            Console.WriteLine("2. Add new location");
            Console.WriteLine("3. Edit location");
            Console.WriteLine("4. Delete location");
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
                    ListAllLocations();
                    NavigationMenu();
                    break;
                case 2:
                    Console.WriteLine("");
                    AddNewLocation();
                    NavigationMenu();
                    break;
                case 3:
                    EditLocation();
                    NavigationMenu();
                    break;
                case 4:
                    DeleteLocation();
                    NavigationMenu();
                    break;
                case 5:
                    Console.WriteLine("You are exiting application. Goodbye.");
                    break;

            }
        }

        private void DeleteLocation()
        {
            Console.WriteLine("");
            Console.WriteLine("---------------");
            Console.WriteLine("delete location");
            Console.WriteLine("---------------");

            ListAllLocations();
            Console.WriteLine("");

            int input = Utility.LoadNumberRange("Choose location to edit: ", "Choose correct!", 1, Locations.Count);

            Console.WriteLine("");
            Console.WriteLine("Location deleted successfully.");

            Locations.RemoveAt(input - 1);
        }

        private void EditLocation()
        {
            Console.WriteLine("");
            Console.WriteLine("-------------");
            Console.WriteLine("edit location");
            Console.WriteLine("-------------");

            ListAllLocations();
            Console.WriteLine("");

            int input = Utility.LoadNumberRange("Choose location to edit: ", "Choose correct!", 1, Locations.Count);
            var location = Locations[input - 1];

            location.Name = Utility.LoadString($"Current name: {location.Name}" + " | New name: ", "Input required!");

            Console.WriteLine("");
            Console.WriteLine("Location edited successfully.");


        }

        private void AddNewLocation()
        {
            Console.WriteLine("");
            Console.WriteLine("------------");
            Console.WriteLine("Add location");
            Console.WriteLine("------------");

            var location = new Location();

            location.Id = Utility.ValidateId();
            location.Name = Utility.LoadString("Add location name: ", "Input required!");

            Console.WriteLine("");
            Console.WriteLine("Location added successfully.");

            Locations.Add(location);
        }

        private void ListAllLocations()
        {
            Console.WriteLine("---------");
            Console.WriteLine("Locations");
            Console.WriteLine("---------");

            var number = 0;
            Locations.ForEach(location =>
            {
                Console.WriteLine(++number + ". " + location);
            });
        }

    }
}
