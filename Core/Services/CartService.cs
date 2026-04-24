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
                await _cartRepo.SaveChangesAsync(); // ← spara cart så Id genereras
            }

            var product = await _productRepo.GetProductByIdAsync(dto.ProductId)
                ?? throw new KeyNotFoundException($"Produkt med id {dto.ProductId} hittades inte.");

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPriceSnapshot = product.DefaultPrice
            };

            await _cartRepo.AddCartItemAsync(cartItem);
            await _cartRepo.SaveChangesAsync();  // ← spara så Id genereras

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
                throw new ArgumentException("En varukorg får kopplas till antingen Assignment eller Job, inte båda.");

            var newCart = new Cart
            {
                UserId = userId,
                AssignmentId = dto.AssignmentId,
                JobId = dto.JobId,
                CreatedAt = DateTime.UtcNow
            };

            await _cartRepo.CreateCartAsync(newCart);
            await _cartRepo.SaveChangesAsync();

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






        public async Task<CartDto?> GetCartByIdAsync(int id, int userId, bool isAdmin)
        {


            var cart = await _cartRepo.GetCartByIdAsync(id);
            if (cart == null) return null;

            if (cart.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("Du får inte se andras varukorgar.");

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
                Status = cart.Status,
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
                CartItems = cartItem,
                Status = cart.Status
            };
        }

        public async Task<List<CartDto>> GetAllByUserIdAsync(int userId)
        {
            var carts = await _cartRepo.GetAllByUserIdAsync(userId);
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



        public async Task UpdateCartItemAsync(UpdateCartItemDto dto, int id, int userId, bool isAdmin)
        {
            var cartItem = await _cartRepo.GetCartItemByIdAsync(id)
                ?? throw new KeyNotFoundException($"CartItem med id {id} hittades inte.");

            if (cartItem.Cart.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("Du får inte ändra andras varukorgs-rader.");

            cartItem.Quantity = dto.Quantity;
            await _cartRepo.UpdateCartItemAsync(cartItem);
            await _cartRepo.SaveChangesAsync();
        }

        public async Task CompleteCartAsync(int id, int userId, bool isAdmin)
        {
            var cart = await _cartRepo.GetCartByIdAsync(id)
                ?? throw new KeyNotFoundException($"Varukorg med id {id} hittades inte.");

            if (cart.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("Du får inte ändra andras varukorgar.");
            cart.Status = CartStatus.Completed;
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepo.UpdateCartAsync(cart);
            await _cartRepo.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(int id, int userId, bool isAdmin)
        {
            var cartItem = await _cartRepo.GetCartItemByIdAsync(id)
                ?? throw new KeyNotFoundException($"CartItem med id {id} hittades inte.");

            if (cartItem.Cart.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("Du får inte ta bort andras varukorgs-rader.");

            await _cartRepo.RemoveCartItemAsync(cartItem);
            await _cartRepo.SaveChangesAsync();
        }


        public async Task DeleteCartAsync(int id, int userId, bool isAdmin)
        {
            var cart = await _cartRepo.GetCartByIdAsync(id)
                ?? throw new KeyNotFoundException($"Varukorg med id {id} hittades inte.");

            if (cart.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("Du får inte radera andras varukorgar.");
            await _cartRepo.DeleteCartAsync(cart);
            await _cartRepo.SaveChangesAsync();
        }
    }
}
