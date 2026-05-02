namespace Inventory_Management.DTOs
{
  public class CreateProductRequest
  {
    public string Name { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public decimal PurchasingPrice { get; set; }
    public int MinimumStockLevel { get; set; }
    public int CategoryId { get; set; }
  }

  public class ProductListItemDto
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
  }

  public class ProductDto
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public decimal PurchasingPrice { get; set; }
    public int MinimumStockLevel { get; set; }
    public int CategoryId { get; set; }
  }

  public class GetAllProductsResponse
  {
    public List<ProductListItemDto> Products { get; set; } = new List<ProductListItemDto>();
  }

  public class UpdateProductRequest
  {
    public string? Name { get; set; }
    public decimal? SellingPrice { get; set; }
    public decimal? PurchasingPrice { get; set; }
    public int? MinimumStockLevel { get; set; }
    public int? CategoryId { get; set; }
  }

  public enum UpdateProductResult
  {
    Updated = 0,
    NotFound = 1,
    InvalidCategory = 2
  }
}