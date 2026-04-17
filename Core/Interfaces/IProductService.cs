using InstallFlow.Data.DTO;

namespace InstallFlow.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<List<ProductDto>> GetAllProductAsync();
        Task<List<ProductDto>> GetProductBySlugAsync(string product);
        Task<ProductDto> CreateProductAsync(CreateProductDto product);

        Task UpdateProductAsync(UpdateProductDto dto, int id);
        Task DeleteProductAsync(int id);




    }
}
