using Inventory_Management.Enums;

namespace Inventory_Management.DTOs
{
  public class CurrentStockReportItemDto
  {
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CurrentStockLevel { get; set; }
    public int MinimumStockLevel { get; set; }
  }

  public class CurrentStockReportResponse
  {
    public List<CurrentStockReportItemDto> Items { get; set; } = new List<CurrentStockReportItemDto>();
  }

  public class LowStockReportResponse
  {
    public List<CurrentStockReportItemDto> Items { get; set; } = new List<CurrentStockReportItemDto>();
  }

  public class ProductMovementHistoryItemDto
  {
    public int StockMovementId { get; set; }
    public DateTime MovementDate { get; set; }
    public MovementType MovementType { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? Notes { get; set; }
  }

  public class ProductMovementHistoryResponse
  {
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public List<ProductMovementHistoryItemDto> Movements { get; set; } = new List<ProductMovementHistoryItemDto>();
  }
}