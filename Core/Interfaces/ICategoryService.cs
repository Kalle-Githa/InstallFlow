using InstallFlow.Data.DTO.Categories;
using Microsoft.AspNetCore.JsonPatch;

namespace InstallFlow.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryListDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<List<CategoryDto>> GetCategoryBySlugAsync(string slug);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        Task<CategoryDto> UpdateCategoryAsync(JsonPatchDocument<UpdateCategoryDto> patchDoc, int id);
        Task RemoveProductFromCategoryAsync(int categoryId, int productId);
        Task DeleteCategoryAsync(int id);

    }
}