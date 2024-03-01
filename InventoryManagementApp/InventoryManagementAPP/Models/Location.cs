using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Represents a Plain Old CLR Object (POCO) class mapped to the database for managing locations.
    /// </summary>
    public class Location : Entity
    {
        /// <summary>
        /// Gets or sets the name of the location in the database.
        /// </summary>
        [Required(ErrorMessage = "Name required!")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the location in the database.
        /// </summary>
        public string? Description { get; set; }
    }
}
