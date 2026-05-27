using Inventory_Management.DTOs;
using Inventory_Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.Controllers
{
  [ApiController]
  [Route("api/reporting")]
  public class ReportingController : ControllerBase
  {
    private readonly IReportingService _reportingService;

    public ReportingController(IReportingService reportingService)
    {
      _reportingService = reportingService;
    }

    [HttpGet("current-stock")]
    public async Task<ActionResult<CurrentStockReportResponse>> GetCurrentStockReport()
    {
      var response = await _reportingService.GetCurrentStockReportAsync();
      return Ok(response);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<LowStockReportResponse>> GetLowStockReport()
    {
      var response = await _reportingService.GetLowStockReportAsync();
      return Ok(response);
    }

    [HttpGet("product/{productId}/movement-history")]
    public async Task<ActionResult<ProductMovementHistoryResponse>> GetProductMovementHistory(int productId)
    {
      var response = await _reportingService.GetProductMovementHistoryAsync(productId);

      if (response == null)
      {
        return NotFound(new { message = "Product not found." });
      }

      return Ok(response);
    }
  }
}