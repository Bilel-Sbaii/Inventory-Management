using Inventory_Management.DTOs;

namespace Inventory_Management.Services.Interfaces
{
  public interface ISupplierService
  {
    Task<GetAllSuppliersResponse> GetAllSuppliersAsync();
    Task<SupplierDto?> GetSupplierById(int id);
    Task<bool> DeleteSupplierAsync(int id);
    Task<SupplierDto> CreateSupplierAsync(CreateSupplierRequest request);
    Task<UpdateSupplierResult> UpdateSupplierAsync(int id, UpdateSupplierRequest request);
  }
}