using System.Security.Claims;
using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstallFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllCarts()
        {
            var carts = await _cartService.GetAllCartsAsync();
            return Ok(carts);
        }






        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Cart(int id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null)
            {
                return NotFound();

            }


            return Ok(cart);

        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> CartByUserId(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound();

            }


            return Ok(cart);

        }

        [Authorize]
        [HttpPost("items")]

        public async Task<IActionResult> AddCartItem(AddCartItemDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cart = await _cartService.AddCartItemAsync(dto, userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }
        

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCart(CreateCartDto dto)
        {
            try
            {
                var cart = await _cartService.CreateCartAsync(dto);
                return CreatedAtAction(nameof(CartByUserId), new { userId = cart.UserId }, cart);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteCart(int id)
        {
            await _cartService.CompleteCartAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPatch("items/{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, UpdateCartItemDto dto)
        {
            await _cartService.UpdateCartItemAsync(dto, id);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            await _cartService.RemoveCartItemAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            await _cartService.DeleteCartAsync(id);
            return NoContent();
        }











    }


}
