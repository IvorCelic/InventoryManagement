namespace ConsoleApp1.Models
{
    internal class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsUnique { get; set; }
        public Person Person { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
