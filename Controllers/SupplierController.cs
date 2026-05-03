using Inventory_Management.DTOs;
using Inventory_Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.Controllers
{
  [ApiController]
  [Route("api/supplier")]
  public class SupplierController : ControllerBase
  {
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
      _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<GetAllSuppliersResponse>> GetAllSuppliers()
    {
      var response = await _supplierService.GetAllSuppliersAsync();
      return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierById(int id)
    {
      var supplier = await _supplierService.GetSupplierById(id);
      if (supplier == null) return NotFound();

      return Ok(supplier);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
      var deleted = await _supplierService.DeleteSupplierAsync(id);
      if (!deleted) return NotFound();

      return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierRequest request)
    {
      var result = await _supplierService.UpdateSupplierAsync(id, request);

      if (result == UpdateSupplierResult.NotFound)
      {
        return NotFound();
      }

      return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] CreateSupplierRequest request)
    {
      if (string.IsNullOrWhiteSpace(request.Name))
      {
        return BadRequest();
      }

      var created = await _supplierService.CreateSupplierAsync(request);
      return CreatedAtAction(nameof(GetSupplierById), new { id = created.Id }, created);
    }
  }
}