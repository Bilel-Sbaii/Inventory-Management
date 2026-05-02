namespace Inventory_Management.DTOs
{
  public class CreateCategoryRequest
  {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
  }

  public class CategoryDto
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
  }

  public class GetAllCategoriesResponse
  {
    public List<string> Categories { get; set; } = new List<string>();
  }

  public class UpdateCategoryRequest
  {
    public string? Name { get; set; }
    public string? Description { get; set; }
  }
}