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
        public async Task<IActionResult> GetAllProducts([FromQuery] string? slug, [FromQuery] ProductType? type)
        {
            if (slug != null)
            {
                var bySlug = await _productService.GetProductBySlugAsync(slug);
                return Ok(bySlug);
            }

            if (type != null)
            {
                var byType = await _productService.GetProductsByTypeAsync(type.Value);
                return Ok(byType);
            }

            var products = await _productService.GetAllProductAsync();
            return Ok(products);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();


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
            // → id:t för den nyskapade produkten → bygger URL:en
            // → product hela produktobjektet → skickas som JSON-body i svaret

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
