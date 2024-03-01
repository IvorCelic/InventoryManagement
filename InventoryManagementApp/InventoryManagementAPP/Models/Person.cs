using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Represents a person entity in the inventory management system.
    /// </summary>
    public class Person : Entity
    {
        /// <summary>
        /// Gets or sets the first name of the person.
        /// </summary>
        [Required(ErrorMessage = "This field is required.")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the person.
        /// </summary>
        [Required(ErrorMessage = "This field is required.")]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the person.
        /// </summary>
        [Required(ErrorMessage = "This field is required.")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the person.
        /// </summary>
        [Required(ErrorMessage = "This field is required.")]
        public string? Password { get; set; }
    }
}
