namespace ConsoleApp2.Models
{
    internal class ProductLocation : Entity
    {
        public int Quantity { get; set; }
        public float Price { get; set; }
        // public string Description { get; set; }
        public Product Product { get; set; }
        public Location Location { get; set; }
        public Person Person { get; set; }

        public override string ToString()
        {
            return Product.Name + " in " + Location.Name;
        }


    }
}
