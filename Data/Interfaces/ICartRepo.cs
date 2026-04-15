using InstallFlow.Data.Entities;

namespace InstallFlow.Data.Interfaces
{
    public interface ICartRepo
    {
        Task<Cart?> GetCartByIdAsync(int id);
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart> CreateCartAsync(Cart cart);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(CartItem cartItem);
    }
}