using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Represents a Plain Old CLR Object (POCO) class mapped to the database for managing products when doing transaction.
    /// </summary>
    public class InventoryTransactionItem : Entity
    {
        /// <summary>
        /// Gets or sets the InventoryTransaction in the database
        /// </summary>
        [ForeignKey("inventorytransaction")]
        public InventoryTransaction? InventoryTransaction { get; set; }

        /// <summary>
        /// Gets or sets the Product in the database
        /// </summary>
        [ForeignKey("product")]
        public Product? Product { get; set; }

        /// <summary>
        /// Gets or sets the Warehouse in the database
        /// </summary>
        [ForeignKey("warehouse")]
        public Warehouse? Warehouse { get; set; }

        /// <summary>
        /// Gets or sets Quantity of Product in the database
        /// </summary>
        public int? Quantity { get; set; }
    }
}
