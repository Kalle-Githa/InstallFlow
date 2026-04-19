using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllProducts([FromQuery] string? slug)
        {
            if (slug != null)
            {
                var bySlug = await _productService.GetProductBySlugAsync(slug);
                return Ok(bySlug);
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
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
        {
            var product = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.Id }, product);
            // → id:t för den nyskapade produkten → bygger URL:en
            // → product hela produktobjektet → skickas som JSON-body i svaret

        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody]JsonPatchDocument<UpdateProductDto> dto, int id)
        {
            var product = await _productService.UpdateProductAsync(dto, id);
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
