namespace ConsoleApp2.Models
{
    internal class ProductLocation : Entity
    {
        public int Quantity { get; set; }
        public float Price { get; set; }
        // public string Description { get; set; }
        public List<Product> Products { get; set; }
        public Location Location { get; set; }
        public Person Person { get; set; }

        public ProductLocation()
        {
            Products = new List<Product>();
        }

        public override string ToString()
        {
            return Location.Name;
        }


    }
}
