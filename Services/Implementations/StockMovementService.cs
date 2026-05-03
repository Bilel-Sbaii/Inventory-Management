using Inventory_Management.Data;
using Inventory_Management.DTOs;
using Inventory_Management.Entities;
using Inventory_Management.Enums;
using Inventory_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Services.Implementations
{
  public class StockMovementService : IStockMovementService
  {
    private readonly InventoryManagementDbContext _context;

    public StockMovementService(InventoryManagementDbContext context)
    {
      _context = context;
    }

    public async Task<AddStockMovementResult> AddStockMovement(AddStockMovementTransactionRequest request)
    {
      var supplierExist = await _context.Suppliers
        .AsNoTracking()
        .AnyAsync(s => s.Id == request.SupplierId);

      if (!supplierExist)
      {
        return AddStockMovementResult.InvalidSupplier;
      }

      if (request.Details == null || request.Details.Count == 0)
      {
        return AddStockMovementResult.InvalidProduct;
      }

      var validProducts = await ValidateProducts(request.MovementType, request.Details);

      if (!validProducts)
      {
        return AddStockMovementResult.InvalidProduct;
      }

      var stockMovement = new StockMovement
      {
        SupplierId = request.SupplierId,
        Type = request.MovementType,
        MovementDate = request.MovementDate ?? DateTime.UtcNow,
        Notes = request.Notes,
        Details = request.Details.Select(d => new StockMovementDetail
        {
          ProductId = d.ProductId,
          Quantity = d.Quantity,
          UnitPrice = d.UnitPrice
        }).ToList()
      };

      await _context.StockMovements.AddAsync(stockMovement);
      await _context.SaveChangesAsync();

      return AddStockMovementResult.Success;
    }

    private async Task<bool> ValidateProducts(MovementType movementType, List<AddStockMovementDetailRequest> products)
    {
      //ensure that the product ids are valid (exist as products in the Products entity)
      var requestedProductIds = products
        .Select(p => p.ProductId)
        .Distinct()
        .ToList();

      var existingProductIds = await _context.Products
        .AsNoTracking()
        .Where(p => requestedProductIds.Contains(p.Id))
        .Select(p => p.Id)
        .ToListAsync();

      if (existingProductIds.Count != requestedProductIds.Count)
      {
        return false;
      }

      if (movementType != MovementType.Out)
      {
        return true;
      }

      //for each product create its requested sum of quantity
      var requestedQuantityByProduct = products
        .GroupBy(p => p.ProductId)
        .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

      //create a productStockSummary for each requested product
      var stockByProduct = await _context.StockMovementDetails
        .AsNoTracking()
        .Where(md => requestedProductIds.Contains(md.ProductId))
        .GroupBy(md => md.ProductId)
        .Select(g => new ProductStockSummary
        {
          ProductId = g.Key,
          TotalIncoming = g.Sum(md => md.StockMovement.Type == MovementType.In ? md.Quantity : 0),
          TotalProcessed = g.Sum(md => md.Quantity)
        })
        .ToDictionaryAsync(x => x.ProductId);

      foreach (var requestItem in requestedQuantityByProduct)
      {
        stockByProduct.TryGetValue(requestItem.Key, out var summary);

        var totalIncoming = summary?.TotalIncoming ?? 0;
        var totalProcessed = summary?.TotalProcessed ?? 0;
        var totalOutgoing = totalProcessed - totalIncoming;
        var currentStockQuantity = totalIncoming - totalOutgoing;

        if (currentStockQuantity < requestItem.Value)
        {
          return false;
        }
      }

      return true;
    }

    private class ProductStockSummary
    {
      public int ProductId { get; set; }
      public int TotalIncoming { get; set; }
      public int TotalProcessed { get; set; }
    }
  }
}
