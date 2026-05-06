using InstallFlow.Data.DTO.Cart;

namespace InstallFlow.Core.Interfaces
{
    public interface ICartService
    {
        Task<List<CartDto>> GetAllCartsAsync(bool isAdmin);
        Task<CartDto> GetCartByIdAsync(int id, int userId, bool isAdmin);
        Task<CartDto> GetCartByUserIdAsync(int userId);
        Task<List<CartDto>> GetAllByUserIdAsync(int userId);
        Task<CartDto> CreateCartAsync(CreateCartDto dto, int userId);
        Task<CartItemDto> AddCartItemAsync(AddCartItemDto cartItem, int userId);
        Task UpdateCartItemAsync(UpdateCartItemDto dto, int id, int userId, bool isAdmin);
        Task CompleteCartAsync(int id, int userId, bool isAdmin);
        Task RemoveCartItemAsync(int id, int userId, bool isAdmin);
        Task DeleteCartAsync(int id, int userId, bool isAdmin);


    }
}
