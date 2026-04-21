using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Cart;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Enums;
using InstallFlow.Data.Interfaces;

namespace InstallFlow.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _cartRepo;
        private readonly IProductRepo _productRepo;
        public CartService(ICartRepo cartRepo, IProductRepo productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }


        public async Task<CartItemDto> AddCartItemAsync(AddCartItemDto dto, int userId)
        {


            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = await _cartRepo.CreateCartAsync(new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Hämta produkten för att snappa priset
            var product = await _productRepo.GetProductByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Produkten hittades inte.");

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPriceSnapshot = product.DefaultPrice  // snappar priset nu
            };

            await _cartRepo.AddCartItemAsync(cartItem);

            var created = await _cartRepo.GetCartItemByIdAsync(cartItem.Id);

            return new CartItemDto
            {
                Id = created!.Id,
                ProductId = created.ProductId,
                ProductName = created.Product.Name,
                Quantity = created.Quantity,
                UnitPriceSnapshot = created.UnitPriceSnapshot

            };
        }



        public async Task<CartDto?> CreateCartAsync(CreateCartDto dto, int userId)
        {
            // Kolla om användaren redan har en aktiv cart
            var existingCart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (existingCart != null)
                return null;

            if (dto.AssignmentId.HasValue && dto.JobId.HasValue)
                throw new ArgumentException();

            var newCart = new Cart
            {
                UserId = userId,
                AssignmentId = dto.AssignmentId,
                JobId = dto.JobId,
                CreatedAt = DateTime.UtcNow
            };

            await _cartRepo.CreateCartAsync(newCart);

            return new CartDto
            {
                Id = newCart.Id,
                UserId = newCart.UserId,
                AssignmentId = newCart.AssignmentId,
                JobId = newCart.JobId,
                Status = newCart.Status,
                CreatedAt = newCart.CreatedAt,
                UpdatedAt = newCart.UpdatedAt
            };
        }
        public async Task<List<CartDto>?> GetAllCartsAsync(bool isAdmin)
        {
            if (!isAdmin) return null;




            var carts = await _cartRepo.GetAllCartsAsync();

            return carts.Select(cart => new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                AssignmentId = cart.AssignmentId,
                JobId = cart.JobId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Status = cart.Status,
                CartItems = cart.CartItems.Select(x => new CartItemDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    UnitPriceSnapshot = x.UnitPriceSnapshot
                }).ToList()
            }).ToList();
        }






        public async Task<CartDto?> GetCartByIdAsync(int id)
        {
            var cart = await _cartRepo.GetCartByIdAsync(id);
            if (cart == null)
                return null;

            var cartItem = cart.CartItems.Select(x => new CartItemDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Quantity = x.Quantity,
                UnitPriceSnapshot = x.UnitPriceSnapshot

            }).ToList();

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                AssignmentId = cart.AssignmentId,
                JobId = cart.JobId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cartItem
            };
        }


        public async Task<CartDto?> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
                return null;

            var cartItem = cart.CartItems.Select(x => new CartItemDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Quantity = x.Quantity,
                UnitPriceSnapshot = x.UnitPriceSnapshot

            }).ToList();

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                AssignmentId = cart.AssignmentId,
                JobId = cart.JobId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cartItem
            };
        }



        public async Task UpdateCartItemAsync(UpdateCartItemDto dto, int id)
        {
            var cartItem = await _cartRepo.GetCartItemByIdAsync(id);
            if (cartItem == null)
                throw new KeyNotFoundException("CartItem hittades inte.");

            cartItem.Quantity = dto.Quantity;
            await _cartRepo.UpdateCartItemAsync(cartItem);
        }

        public async Task CompleteCartAsync(int id)
        {
            var cart = await _cartRepo.GetCartByIdAsync(id);
            if (cart == null)
                throw new KeyNotFoundException("Cart hittades inte.");

            cart.Status = CartStatus.Completed;
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepo.UpdateCartAsync(cart);
        }

        public async Task RemoveCartItemAsync(int id)
        {
            var cartItem = await _cartRepo.GetCartItemByIdAsync(id);
            if (cartItem == null)
                throw new KeyNotFoundException("CartItem hittades inte.");

            await _cartRepo.RemoveCartItemAsync(cartItem);
        }

        public async Task DeleteCartAsync(int id)
        {
            var cart = await _cartRepo.GetCartByIdAsync(id);
            if (cart == null)
                throw new KeyNotFoundException("Cart hittades inte.");

            await _cartRepo.DeleteCartAsync(cart);
        }
    }
}
