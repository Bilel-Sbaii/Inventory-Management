using Inventory_Management.Enums;

namespace Inventory_Management.Entities
{
  public class StockMovement
  {
    public int Id { get; set; }

    public MovementType Type { get; set; }
    public DateTime MovementDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public ICollection<StockMovementDetail> Details { get; set; } = new List<StockMovementDetail>();
  }
}
