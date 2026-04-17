using InstallFlow.Data.Entities;

namespace InstallFlow.Data.Interfaces
{
    public interface ICartRepo
    {
        Task<List<Cart>> GetAllCartsAsync();
        Task<Cart?> GetCartByIdAsync(int id);
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<CartItem?> GetCartItemByIdAsync(int id);
        Task<Cart> CreateCartAsync(Cart cart);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task UpdateCartAsync(Cart cart);
        Task RemoveCartItemAsync(CartItem cartItem);
        Task DeleteCartAsync(Cart cart);
    }
}