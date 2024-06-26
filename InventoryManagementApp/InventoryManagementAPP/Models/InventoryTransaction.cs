﻿using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Represents a Plain Old CLR Object (POCO) class mapped to the database for managing inventory transactions.
    /// </summary>
    public class InventoryTransaction : Entity
    {
        /// <summary>
        /// Gets or sets the Employee in the database.
        /// </summary>
        [ForeignKey("employee")]
        public Employee? Employee { get; set; }
        /// <summary>
        /// Gets or sets the transaction status in the database.
        /// </summary>
        [ForeignKey("transactionstatus")]
        public TransactionStatus? TransactionStatus { get; set; }
        /// <summary>
        /// Gets or sets the transaction datetime in the database.
        /// </summary>
        public DateTime? TransactionDate { get; set; }
        /// <summary>
        /// Gets or sets the additional details in the database.
        /// </summary>
        public string? AdditionalDetails { get; set; }

    }
}
