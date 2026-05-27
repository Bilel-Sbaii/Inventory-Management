using Inventory_Management.Data;
using Inventory_Management.DTOs;
using Inventory_Management.Enums;
using Inventory_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Services.Implementations
{
  public class ReportingService : IReportingService
  {
    private readonly InventoryManagementDbContext _context;

    public ReportingService(InventoryManagementDbContext context)
    {
      _context = context;
    }

    public async Task<CurrentStockReportResponse> GetCurrentStockReportAsync()
    {
      var items = await GetCurrentStockItemsAsync();

      return new CurrentStockReportResponse
      {
        Items = items
      };
    }

    public async Task<LowStockReportResponse> GetLowStockReportAsync()
    {
      var items = await GetCurrentStockItemsAsync();

      var lowStockItems = items
        .Where(i => i.CurrentStockLevel <= i.MinimumStockLevel)
        .OrderBy(i => i.CurrentStockLevel)
        .ThenBy(i => i.ProductName)
        .ToList();

      return new LowStockReportResponse
      {
        Items = lowStockItems
      };
    }

    public async Task<ProductMovementHistoryResponse?> GetProductMovementHistoryAsync(int productId)
    {
      var product = await _context.Products
        .AsNoTracking()
        .Where(p => p.Id == productId)
        .Select(p => new
        {
          p.Id,
          p.Name
        })
        .SingleOrDefaultAsync();

      if (product == null)
      {
        return null;
      }

      var movements = await _context.StockMovementDetails
        .AsNoTracking()
        .Where(md => md.ProductId == productId)
        .OrderByDescending(md => md.StockMovement.MovementDate)
        .ThenByDescending(md => md.StockMovementId)
        .Select(md => new ProductMovementHistoryItemDto
        {
          StockMovementId = md.StockMovementId,
          MovementDate = md.StockMovement.MovementDate,
          MovementType = md.StockMovement.Type,
          Quantity = md.Quantity,
          UnitPrice = md.UnitPrice,
          SupplierId = md.StockMovement.SupplierId,
          SupplierName = md.StockMovement.Supplier != null ? md.StockMovement.Supplier.Name : null,
          Notes = md.StockMovement.Notes
        })
        .ToListAsync();

      return new ProductMovementHistoryResponse
      {
        ProductId = product.Id,
        ProductName = product.Name,
        Movements = movements
      };
    }

    private async Task<List<CurrentStockReportItemDto>> GetCurrentStockItemsAsync()
    {
      var stockByProduct = await _context.StockMovementDetails
        .AsNoTracking()
        .GroupBy(md => md.ProductId)
        .Select(g => new
        {
          ProductId = g.Key,
          CurrentStockLevel = g.Sum(md => md.StockMovement.Type == MovementType.In ? md.Quantity : -md.Quantity)
        })
        .ToDictionaryAsync(x => x.ProductId, x => x.CurrentStockLevel);

      var products = await _context.Products
        .AsNoTracking()
        .Select(p => new
        {
          p.Id,
          p.Name,
          p.MinimumStockLevel
        })
        .ToListAsync();

      return products
        .Select(p =>
        {
          stockByProduct.TryGetValue(p.Id, out var currentStockLevel);

          return new CurrentStockReportItemDto
          {
            ProductId = p.Id,
            ProductName = p.Name,
            CurrentStockLevel = currentStockLevel,
            MinimumStockLevel = p.MinimumStockLevel
          };
        })
        .OrderBy(i => i.ProductName)
        .ToList();
    }
  }
}