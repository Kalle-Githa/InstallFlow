using InstallFlow.Data.DTO.Products;
using InstallFlow.Data.Enums;
using Microsoft.AspNetCore.JsonPatch;

namespace InstallFlow.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id); // vem som helst
        Task<List<ProductDto>> GetAllProductAsync(); // vem som helst
        Task<List<ProductDto>> GetProductBySlugAsync(string product); // vem som helst
        Task<List<ProductDto>> GetProductsByTypeAsync(ProductType type);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto, int userId);

        Task<ProductDto> UpdateProductAsync(JsonPatchDocument<UpdateProductDto> patchDoc, int id, int userId, bool isAdmin);
        Task DeleteProductAsync(int id);


    }
}
