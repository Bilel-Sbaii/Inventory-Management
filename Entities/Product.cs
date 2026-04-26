namespace Inventory_Management.Entities
{
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal CurrentPrice { get; set; }
    public int MinimumStockLevel { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null;

    public ICollection<StockMovementDetail> StockMovementDetails { get; set; } = new List<StockMovementDetail>();
  }
}
