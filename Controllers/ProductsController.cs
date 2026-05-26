using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Products;
using InstallFlow.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InstallFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] string? slug,
            [FromQuery] string? search,
            [FromQuery] ProductType? type,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (!string.IsNullOrWhiteSpace(slug))
            {
                var bySlug = await _productService.GetProductBySlugAsync(slug);
                return Ok(bySlug);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var bySearch = await _productService.SearchProductsAsync(search, page, pageSize);
                return Ok(bySearch);
            }

            if (type.HasValue)
            {
                var byType = await _productService.GetProductsByTypeAsync(type.Value);
                return Ok(byType);
            }

            var products = await _productService.GetAllProductAsync(page, pageSize);
            return Ok(products);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            return Ok(product);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var product = await _productService.CreateProductAsync(dto, userId);
            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.Id }, product);

        }


        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] JsonPatchDocument<UpdateProductDto> dto, int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");
            var product = await _productService.UpdateProductAsync(dto, id, userId, isAdmin);

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);

            return NoContent();
        }
    }
}
