using Inventory_Management.DTOs;
using Inventory_Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.Controllers
{
  [ApiController]
  [Route("api/stockMovement")]
  public class StockMovementController : ControllerBase
  {
    private readonly IStockMovementService _stockMovementService;

    public StockMovementController(IStockMovementService stockMovementService)
    {
      _stockMovementService = stockMovementService;
    }

    [HttpPost]
    public async Task<IActionResult> AddStockMovementTransaction([FromBody] AddStockMovementTransactionRequest request)
    {
      if (request == null)
      {
        return BadRequest();
      }

      var result = await _stockMovementService.AddStockMovement(request);

      if (result == AddStockMovementResult.InvalidSupplier)
      {
        return BadRequest(new { message = "invalid Supplier" });
      }

      if (result == AddStockMovementResult.InvalidProduct)
      {
        return BadRequest(new { message = "invalid quantity/id for Product" });
      }

      return Ok();
    }
  }
}
