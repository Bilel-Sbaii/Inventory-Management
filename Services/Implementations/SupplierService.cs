using Inventory_Management.Data;
using Inventory_Management.DTOs;
using Inventory_Management.Entities;
using Inventory_Management.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Services.Implementations
{
  public class SupplierService : ISupplierService
  {
    private readonly InventoryManagementDbContext _context;

    public SupplierService(InventoryManagementDbContext context)
    {
      _context = context;
    }

    public async Task<GetAllSuppliersResponse> GetAllSuppliersAsync()
    {
      var supplierNames = await _context.Suppliers
        .AsNoTracking()
        .Select(s => s.Name)
        .ToListAsync();

      return new GetAllSuppliersResponse { Suppliers = supplierNames };
    }

    public async Task<SupplierDto?> GetSupplierById(int id)
    {
      return await _context.Suppliers
        .AsNoTracking()
        .Where(s => s.Id == id)
        .Select(s => new SupplierDto
        {
          Id = s.Id,
          Name = s.Name
        })
        .SingleOrDefaultAsync();
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
      var supplier = await _context.Suppliers.FindAsync(id);
      if (supplier != null)
      {
        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
      }

      return supplier != null;
    }

    public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierRequest request)
    {
      var supplier = new Supplier
      {
        Name = request.Name
      };

      _context.Suppliers.Add(supplier);
      await _context.SaveChangesAsync();

      return new SupplierDto
      {
        Id = supplier.Id,
        Name = supplier.Name
      };
    }

    // partial update
    public async Task<UpdateSupplierResult> UpdateSupplierAsync(int id, UpdateSupplierRequest request)
    {
      var supplier = await _context.Suppliers.FindAsync(id);
      if (supplier == null) return UpdateSupplierResult.NotFound;

      if (!string.IsNullOrWhiteSpace(request.Name))
      {
        supplier.Name = request.Name;
      }

      await _context.SaveChangesAsync();
      return UpdateSupplierResult.Updated;
    }
  }
}