using Inventory_Management.DTOs;
namespace Inventory_Management.Services.Interfaces
{
  public interface ICategoryService
  {
    Task<GetAllCategoriesResponse> GetAllCategoriesAsync();
    Task<CategoryDto?> GetCategoryById(int id);
    Task<bool> DeleteCategoryAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request);
    Task<bool> UpdateCategoryAsync(int id, UpdateCategoryRequest request);
  }
}
