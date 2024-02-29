using InventoryManagementAPP.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Data
{
    /// <summary>
    /// This is directory where I will map datasets and ways of connecting in database
    /// </summary>
    public class InventoryManagementContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public InventoryManagementContext(DbContextOptions<InventoryManagementContext> options) : base (options)
        {

        }

        /// <summary>
        /// Locations in database
        /// </summary>
        public DbSet<Location> Locations { get; set; }
    }
}
