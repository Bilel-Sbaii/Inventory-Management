using Inventory_Management.DTOs;

namespace Inventory_Management.Services.Interfaces
{
  public interface IReportingService
  {
    Task<CurrentStockReportResponse> GetCurrentStockReportAsync();
    Task<LowStockReportResponse> GetLowStockReportAsync();
    Task<ProductMovementHistoryResponse?> GetProductMovementHistoryAsync(int productId);
  }
}