using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var isAdmin = User.IsInRole("Admin");
            var carts = await _cartService.GetAllCartsAsync(isAdmin);
            return Ok(carts);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Cart(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");
            var cart = await _cartService.GetCartByIdAsync(id, userId, isAdmin);
            if (cart == null) return NotFound();


            return Ok(cart);
        }




        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyCart()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null) return NotFound();

            return Ok(cart);

        }


        [Authorize]
        [HttpGet("me/history")]
        public async Task<IActionResult> GetMyHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var carts = await _cartService.GetAllByUserIdAsync(userId);
            return Ok(carts);
        }


        [Authorize]
        [HttpPost("items")]

        public async Task<IActionResult> AddCartItem(AddCartItemDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cart = await _cartService.AddCartItemAsync(dto, userId);

            return Ok(cart);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCart(CreateCartDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var cart = await _cartService.CreateCartAsync(dto, userId);
            if (cart == null)
                return Conflict(new { message = "Användaren har redan en aktiv varukorg." });

            return CreatedAtAction(nameof(GetMyCart), null, cart);
        }

        [Authorize]
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteCart(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");
            await _cartService.CompleteCartAsync(id, userId, isAdmin);
            return NoContent();
        }

        [Authorize]
        [HttpPatch("items/{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, UpdateCartItemDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");
            await _cartService.UpdateCartItemAsync(dto, id, userId, isAdmin);
            return NoContent();
        }


        [Authorize]
        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");
            await _cartService.RemoveCartItemAsync(id, userId, isAdmin);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");
            await _cartService.DeleteCartAsync(id, userId, isAdmin);
            return NoContent();
        }

    }


}
