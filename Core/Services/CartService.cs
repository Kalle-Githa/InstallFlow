using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using InstallFlow.Data.Entities;
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

        public async Task<CartItemDto> AddCartItemAsync(AddCartItemDto dto)
        {
            var product = await _productRepo.GetProductByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Produkten hittades inte.");

            var cartItem = new CartItem
            {
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPriceSnapshot = product.DefaultPrice
            };

            await _cartRepo.AddCartItemAsync(cartItem);

            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                Quantity = cartItem.Quantity,
                UnitPriceSnapshot = cartItem.UnitPriceSnapshot
            };
        }



        public async Task<CartDto> CreateCartAsync(CreateCartDto cart)
        {
            if (cart.AssignmentId.HasValue && cart.JobId.HasValue)
                throw new ArgumentException();


            var newCart = new Cart
            {
                UserId = cart.UserId,
                AssignmentId = cart.AssignmentId,
                JobId = cart.JobId,
                CreatedAt = DateTime.UtcNow

            };
            await _cartRepo.CreateCartAsync(newCart);

            return new CartDto
            {
                Id = newCart.Id,
                UserId = newCart.UserId,
                AssignmentId = newCart.AssignmentId,
                JobId = newCart.JobId,
                CreatedAt = newCart.CreatedAt,
                UpdatedAt = newCart.UpdatedAt


            };


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

        public async Task RemoveCartItemAsync(int id)
        {
            var cartItem = await _cartRepo.GetCartItemByIdAsync(id);
            if (cartItem == null)
                throw new KeyNotFoundException("CartItem hittades inte.");

            await _cartRepo.RemoveCartItemAsync(cartItem);
        }

        public async Task UpdateCartItemAsync(UpdateCartItemDto dto, int id)
        {
            var cartItem = await _cartRepo.GetCartItemByIdAsync(id);
            if (cartItem == null)
                throw new KeyNotFoundException("CartItem hittades inte.");

            cartItem.Quantity = dto.Quantity;
            await _cartRepo.UpdateCartItemAsync(cartItem);
        }
    }
}
