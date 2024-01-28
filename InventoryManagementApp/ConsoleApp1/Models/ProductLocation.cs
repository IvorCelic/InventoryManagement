using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    internal class ProductLocation : Entity
    {
        private readonly int Quantity;
        private readonly float Price;
        private readonly string Description;
        private readonly Product Product;
        private readonly Location Location;
        private readonly Person Person;

    }
}
