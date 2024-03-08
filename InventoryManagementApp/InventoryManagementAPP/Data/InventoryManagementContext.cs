using InventoryManagementAPP.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Data
{
    /// <summary>
    /// Represents the context for mapping datasets and defining database connections.
    /// </summary>
    public class InventoryManagementContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryManagementContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public InventoryManagementContext(DbContextOptions<InventoryManagementContext> options) : base(options)
        {

        }

        /// <summary>
        /// Gets or sets the dataset representing locations in the database.
        /// </summary>
        public DbSet<Location> Locations { get; set; }

        /// <summary>
        /// Gets or sets the dataset representing persons in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}
