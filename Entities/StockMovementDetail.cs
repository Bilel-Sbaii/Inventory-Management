namespace Inventory_Management.Entities
{
  public class StockMovementDetail
  {
    public int Id { get; set; }

    public int StockMovementId { get; set; }
    public StockMovement StockMovement { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal PriceAtMovement { get; set; }
  }
}
