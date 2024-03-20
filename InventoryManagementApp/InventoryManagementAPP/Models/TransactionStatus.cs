namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Represents a Plain Old CLR Object (POCO) class mapped to the database for managing inventory transaction statuses.
    /// </summary>
    public class TransactionStatus : Entity
    {
        /// <summary>
        /// Gets status name from the database
        /// </summary>
        public string? StatusName { get; set; }
    }
}
