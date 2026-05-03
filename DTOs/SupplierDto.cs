namespace Inventory_Management.DTOs
{
  public class CreateSupplierRequest
  {
    public string Name { get; set; } = string.Empty;
  }

  public class SupplierDto
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
  }

  public class GetAllSuppliersResponse
  {
    public List<string> Suppliers { get; set; } = new List<string>();
  }

  public class UpdateSupplierRequest
  {
    public string? Name { get; set; }
  }

  public enum UpdateSupplierResult
  {
    Updated = 0,
    NotFound = 1,
  }
}