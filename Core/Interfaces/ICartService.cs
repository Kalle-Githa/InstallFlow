using InstallFlow.Data.DTO;

namespace InstallFlow.Core.Interfaces
{
    public interface ICartService
    {
        Task<List<CartDto>> GetAllCartsAsync();
        Task<CartDto?> GetCartByIdAsync(int id);
        Task<CartDto?> GetCartByUserIdAsync(int userId);
        Task<CartDto> CreateCartAsync(CreateCartDto cart);
        Task<CartItemDto> AddCartItemAsync(AddCartItemDto cartItem,int userId);
        Task UpdateCartItemAsync(UpdateCartItemDto dto, int id);
        Task CompleteCartAsync(int id);
        Task RemoveCartItemAsync(int id);
        Task DeleteCartAsync(int id);


    }
}
