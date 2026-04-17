using InstallFlow.Data.DTO;

namespace InstallFlow.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto product);
        Task UpdateProductItemAsync(UpdateProductDto dto, int id);
        Task DeleteProductAsync(int id);




    }
}
