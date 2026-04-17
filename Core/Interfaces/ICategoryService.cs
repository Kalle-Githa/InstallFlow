using InstallFlow.Data.DTO;

namespace InstallFlow.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<List<CategoryDto>> GetCategoryBySlugAsync(string slug);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        Task UpdateCategoryAsync(UpdateCategoryDto dto, int id);
        Task DeleteCategoryAsync(int id);
    }
}