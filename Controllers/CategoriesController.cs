using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
namespace InstallFlow.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? slug)
    {
        if (slug != null)
        {
            var bySlug = await _categoryService.GetCategoryBySlugAsync(slug);
            return Ok(bySlug);
        }
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var category = await _categoryService.CreateCategoryAsync(dto);
        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id }, category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCategory([FromBody] JsonPatchDocument<UpdateCategoryDto> patchDoc, int id)
    {
        var category = await _categoryService.UpdateCategoryAsync(patchDoc, id);

        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{categoryId}/products/{productId}")]
    public async Task<IActionResult> RemoveProductFromCategory(int categoryId, int productId)
    {
        await _categoryService.RemoveProductFromCategoryAsync(categoryId, productId);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }
}