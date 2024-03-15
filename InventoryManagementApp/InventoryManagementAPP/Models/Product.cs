using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Represents a Plain Old CLR Object (POCO) class mapped to the database for managing products.
    /// </summary>
    public class Product : Entity
    {
        /// <summary>
        /// Gets or sets the name of the product in the database.
        /// </summary>
        [Required(ErrorMessage = "Name required!")]
        public string? ProductName { get; set; }

        /// <summary>
        /// Gets or sets the description of the product in the database.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the description of the product in the database
        /// </summary>
        public bool? IsUnitary { get; set; }
    }
}
