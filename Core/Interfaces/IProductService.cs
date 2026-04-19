using InstallFlow.Data.DTO;
using Microsoft.AspNetCore.JsonPatch;

namespace InstallFlow.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id); // vem som helst
        Task<List<ProductDto>> GetAllProductAsync(); // vem som helst
        Task<List<ProductDto>> GetProductBySlugAsync(string product); // vem som helst
        Task<ProductDto> CreateProductAsync(CreateProductDto product); // en tekniker eller kanske bara admin

       
        Task DeleteProductAsync(int id); // en tekniker kanske bara admin
        Task <ProductDto> UpdateProductAsync(JsonPatchDocument<UpdateProductDto> dto, int id); // en tekniker kanske bara admin





    }
}
