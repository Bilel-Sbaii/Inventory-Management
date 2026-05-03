using Inventory_Management.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Management.DTOs
{
  public class AddStockMovementTransactionRequest
  {
    [Required]
    public MovementType MovementType { get; set; }

    public DateTime? MovementDate { get; set; }

    public string? Notes { get; set; }

    [Range(1, int.MaxValue)]
    public int SupplierId { get; set; }

    [MinLength(1)]
    public List<AddStockMovementDetailRequest> Details { get; set; } = new();
  }

  public class AddStockMovementDetailRequest
  {
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }
  }

  public class AddStockMovementTransactionResponse
  {
    public int StockMovementId { get; set; }
    public MovementType Type { get; set; }
  }

  public enum AddStockMovementResult
  {
    InvalidProduct = 0,
    InvalidSupplier = 1,
    Success = 2
  }
}
