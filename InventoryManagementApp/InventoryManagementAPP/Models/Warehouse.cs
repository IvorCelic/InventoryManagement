using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Represents a Plain Old CLR Object (POCO) class mapped to the database for managing warehouses.
    /// </summary>
    public class Warehouse : Entity
    {
        /// <summary>
        /// Gets or sets the name of the location in the database.
        /// </summary>
        [Required(ErrorMessage = "Name required!")]
        public string? WarehouseName { get; set; }

        /// <summary>
        /// Gets or sets the description of the location in the database.
        /// </summary>
        public string? Description { get; set; }
    }
}
