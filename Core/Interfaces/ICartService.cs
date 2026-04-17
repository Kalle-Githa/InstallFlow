using InstallFlow.Data.DTO;

namespace InstallFlow.Core.Interfaces
{
    public interface ICartService
    {
        Task<CartDto?> GetCartByIdAsync(int id);
        Task<CartDto?> GetCartByUserIdAsync(int userId);
        Task<CartDto> CreateCartAsync(CreateCartDto cart);
        Task<CartItemDto> AddCartItemAsync(AddCartItemDto cartItem);
        Task UpdateCartItemAsync(UpdateCartItemDto dto, int id);
        Task RemoveCartItemAsync(int id);


    }
}
