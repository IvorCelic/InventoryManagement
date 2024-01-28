namespace ConsoleApp1.Models
{
    internal class Person : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

    }
}
