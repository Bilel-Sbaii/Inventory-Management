using Inventory_Management.DTOs;

namespace Inventory_Management.Services.Interfaces
{
  public interface IProductService
  {
    Task<GetAllProductsResponse> GetAllProductsAsync();
    Task<ProductDto?> GetProductById(int id);
    Task<bool> DeleteProductAsync(int id);
    Task<ProductDto?> CreateProductAsync(CreateProductRequest request);
    Task<UpdateProductResult> UpdateProductAsync(int id, UpdateProductRequest request);
  }
}