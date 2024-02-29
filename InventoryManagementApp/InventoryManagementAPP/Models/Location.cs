using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// This is POCO class which is mapped to the database
    /// </summary>
    public class Location : Entity
    {
        /// <summary>
        /// Name in database
        /// </summary>
        [Required(ErrorMessage = "Name required!")]
        public string? Name { get; set; }

        /// <summary>
        /// Description in database
        /// </summary>
        public string? Description { get; set; }
    }
}
