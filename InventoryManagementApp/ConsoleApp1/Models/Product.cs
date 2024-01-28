using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    internal class Product : Entity
    {
        private readonly string Name;
        private readonly string Description;
        private readonly bool IsUnitary; // Is it in one piece or pieces
        private readonly Person Person;

    }
}
