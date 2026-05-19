using InstallFlow.Data.DTO.Products;
using InstallFlow.Data.Enums;
using Microsoft.AspNetCore.JsonPatch;

namespace InstallFlow.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<List<ProductDto>> GetAllProductAsync(int page, int pageSize);
        Task<List<ProductDto>> GetProductBySlugAsync(string product);
        Task<List<ProductDto>> GetProductsByTypeAsync(ProductType type);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm, int page, int pageSize);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto, int userId);
        Task<ProductDto> UpdateProductAsync(JsonPatchDocument<UpdateProductDto> patchDoc, int id, int userId, bool isAdmin);
        Task DeleteProductAsync(int id);


    }
}
