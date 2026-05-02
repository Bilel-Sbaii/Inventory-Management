using Inventory_Management.Data;
using Inventory_Management.DTOs;
using Inventory_Management.Entities;
using Inventory_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Services.Implementations
{
  public class ProductService : IProductService
  {
    private readonly InventoryManagementDbContext _context;

    public ProductService(InventoryManagementDbContext context)
    {
      _context = context;
    }

    public async Task<GetAllProductsResponse> GetAllProductsAsync()
    {
      var products = await _context.Products
        .AsNoTracking()
        .Select(p => new ProductListItemDto
        {
          Id = p.Id,
          Name = p.Name
        })
        .ToListAsync();

      return new GetAllProductsResponse { Products = products };
    }

    public async Task<ProductDto?> GetProductById(int id)
    {
      return await _context.Products
        .AsNoTracking()
        .Where(p => p.Id == id)
        .Select(p => new ProductDto
        {
          Id = p.Id,
          Name = p.Name,
          SellingPrice = p.SellingPrice,
          PurchasingPrice = p.PurchasingPrice,
          MinimumStockLevel = p.MinimumStockLevel,
          CategoryId = p.CategoryId,
        })
        .SingleOrDefaultAsync();
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
      var product = await _context.Products.FindAsync(id);
      if (product != null)
      {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
      }

      return product != null;
    }

    public async Task<ProductDto?> CreateProductAsync(CreateProductRequest request)
    {
      var categoryExists = await _context.Categories
        .AsNoTracking()
        .AnyAsync(c => c.Id == request.CategoryId);

      if (!categoryExists)
      {
        return null;
      }

      var product = new Product
      {
        Name = request.Name,
        SellingPrice = request.SellingPrice,
        PurchasingPrice = request.PurchasingPrice,
        MinimumStockLevel = request.MinimumStockLevel,
        CategoryId = request.CategoryId
      };

      _context.Products.Add(product);
      await _context.SaveChangesAsync();

      return new ProductDto
      {
        Id = product.Id,
        Name = product.Name,
        SellingPrice = product.SellingPrice,
        PurchasingPrice = product.PurchasingPrice,
        MinimumStockLevel = product.MinimumStockLevel,
        CategoryId = product.CategoryId
      };
    }

    public async Task<UpdateProductResult> UpdateProductAsync(int id, UpdateProductRequest request)
    {
      var product = await _context.Products.FindAsync(id);
      if (product == null)
      {
        return UpdateProductResult.NotFound;
      }

      if (request.CategoryId.HasValue)
      {
        var categoryExists = await _context.Categories
          .AsNoTracking()
          .AnyAsync(c => c.Id == request.CategoryId.Value);

        if (!categoryExists)
        {
          return UpdateProductResult.InvalidCategory;
        }

        product.CategoryId = request.CategoryId.Value;
      }

      if (!string.IsNullOrWhiteSpace(request.Name))
      {
        product.Name = request.Name;
      }

      if (request.SellingPrice.HasValue)
      {
        product.SellingPrice = request.SellingPrice.Value;
      }

      if (request.PurchasingPrice.HasValue)
      {
        product.PurchasingPrice = request.PurchasingPrice.Value;
      }

      if (request.MinimumStockLevel.HasValue)
      {
        product.MinimumStockLevel = request.MinimumStockLevel.Value;
      }

      await _context.SaveChangesAsync();
      return UpdateProductResult.Updated;
    }
  }
}