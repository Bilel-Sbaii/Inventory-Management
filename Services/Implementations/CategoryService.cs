using Inventory_Management.Entities;
using Inventory_Management.Data;
using Microsoft.EntityFrameworkCore;
using Inventory_Management.Services.Interfaces;
using Inventory_Management.DTOs;

namespace Inventory_Management.Services.Implementations
{
  public class CategoryService : ICategoryService
  {
    private readonly InventoryManagementDbContext _context;
    public CategoryService(InventoryManagementDbContext context)
    {
      _context = context;
    }
    public async Task<GetAllCategoriesResponse> GetAllCategoriesAsync()
    {
      var categoryNames = await _context.Categories
        .AsNoTracking()
        .Select(c => c.Name)
        .ToListAsync();
      return new GetAllCategoriesResponse { Categories = categoryNames };
    }

    public async Task<CategoryDto?> GetCategoryById(int id)
    {
      return await _context.Categories
        .AsNoTracking()
        .Where(c => c.Id == id)
        .Select(c => new CategoryDto
        {
          Id = c.Id,
          Name = c.Name,
          Description = c.Description,
        })
        .SingleOrDefaultAsync();
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
      var category = await _context.Categories.FindAsync(id);
      if (category != null) {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
      }
      return category != null;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request)
    { 
      var category = new Category
      {
        Name = request.Name,
        Description = request.Description
      };

      _context.Categories.Add(category);
      await _context.SaveChangesAsync();

      return new CategoryDto
      {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description
      };
    }

    //partial update
    public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;
        if(!string.IsNullOrWhiteSpace(request.Name))
        {
          category.Name = request.Name;
        }

        if(!string.IsNullOrWhiteSpace(request.Description))
        {
          category.Description = request.Description;
        }

        await _context.SaveChangesAsync();
        return true;
    }


  }
}
