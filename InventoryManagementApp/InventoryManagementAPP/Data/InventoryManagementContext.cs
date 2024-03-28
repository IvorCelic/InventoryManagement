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
        /// Gets or sets the dataset representing warehouses in the database.
        /// </summary>
        public DbSet<Warehouse> Warehouses { get; set; }

        /// <summary>
        /// Gets or sets the dataset representing employees in the database.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Gets or sets the dataset representing products in the database
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the dataset representing transaction statuses in the database
        /// </summary>
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }

        /// <summary>
        /// Gets or sets the dataset representing inventory transactions in the database
        /// </summary>
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }

        /// <summary>
        /// Gets or sets the dataset representing inventory transaction items in the database
        /// </summary>
        public DbSet<InventoryTransactionItem> InventoryTransactionItems { get; set; }



        /// <summary>
        /// Method for configuring the database schema and relationships between entities in the InventoryManagementContext.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the database model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryTransaction>().HasOne(it => it.Employee);
            modelBuilder.Entity<InventoryTransaction>().HasOne(it => it.TransactionStatus);

            modelBuilder.Entity<InventoryTransactionItem>().HasOne(iti => iti.InventoryTransaction);
            modelBuilder.Entity<InventoryTransactionItem>().HasOne(iti => iti.Product);
            modelBuilder.Entity<InventoryTransactionItem>().HasOne(iti => iti.Warehouse);

            modelBuilder.Entity<InventoryTransaction>()
                .HasMany(it => it.Products)
                .WithMany(p => p.InventoryTransactions)
                .UsingEntity<Dictionary<string, object>>("inventorytransactionitems",
                c => c.HasOne<Product>().WithMany().HasForeignKey("product"),
                c => c.HasOne<InventoryTransaction>().WithMany().HasForeignKey("inventorytransaction"),
                c => c.ToTable("inventorytransactionitems"));
        }
    }
}
