namespace Inventory_Management.Entities
{
  public class Supplier
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
  }
}
