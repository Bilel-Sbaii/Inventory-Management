using Inventory_Management.DTOs;
using Inventory_Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.Controllers
{
  [ApiController]
  [Route("api/product")]
  public class ProductController : ControllerBase
  {
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
      _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<GetAllProductsResponse>> GetAllProducts()
    {
      var response = await _productService.GetAllProductsAsync();
      return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
      var product = await _productService.GetProductById(id);
      if (product == null) return NotFound();

      return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      var deleted = await _productService.DeleteProductAsync(id);
      if (!deleted) return NotFound();

      return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
      var result = await _productService.UpdateProductAsync(id, request);

      if (result == UpdateProductResult.NotFound)
      {
        return NotFound();
      }

      if (result == UpdateProductResult.InvalidCategory)
      {
        return BadRequest(new { message = "Invalid category id." });
      }

      return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductRequest request)
    {
      if (string.IsNullOrWhiteSpace(request.Name))
      {
        return BadRequest(new { message = "Product name is required." });
      }

      var created = await _productService.CreateProductAsync(request);

      if (created == null)
      {
        return BadRequest(new { message = "Invalid category id." });
      }

      return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
    }
  }
}