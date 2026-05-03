using Inventory_Management.DTOs;

namespace Inventory_Management.Services.Interfaces
{
  public interface IStockMovementService
  {
    Task<AddStockMovementResult> AddStockMovement(AddStockMovementTransactionRequest request);
  }
}
