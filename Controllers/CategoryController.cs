using Inventory_Management.DTOs;
using Inventory_Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.Controllers
{
  [ApiController]
  [Route("api/category")]
  public class CategoryController : ControllerBase
  {
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService catergoryService) {
       _categoryService = catergoryService;
    }

    [HttpGet]
    public async Task<ActionResult<GetAllCategoriesResponse>> GetAllCategories(){
      var response = await _categoryService.GetAllCategoriesAsync();
      return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
      var category = await _categoryService.GetCategoryById(id);
      if(category == null) return NotFound();

      return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
      var deleted = await _categoryService.DeleteCategoryAsync(id);
      if(!deleted) return NotFound();

      return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
    {
      var updated = await _categoryService.UpdateCategoryAsync(id, request);
      if(!updated) return NotFound();

      return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
      if (string.IsNullOrWhiteSpace(request.Name))
      {
        return BadRequest();
      }

      var created = await _categoryService.CreateCategoryAsync(request);
      return CreatedAtAction(nameof(GetCategoryById), new { id = created.Id }, created);
    }

  }
}
