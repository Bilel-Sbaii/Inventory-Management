using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Data
{
  public class InventoryManagementDbContext : DbContext
  {
    public DbSet<Entities.Product> Products { get; set; }
    public DbSet<Entities.Category> Categories { get; set; }
    public DbSet<Entities.Supplier> Suppliers { get; set; }
    public DbSet<Entities.StockMovement> StockMovements { get; set; }
    public DbSet<Entities.StockMovementDetail> StockMovementDetails { get; set; }
    public InventoryManagementDbContext(DbContextOptions<InventoryManagementDbContext> options) : base(options)
    {
    }
  }
}
