using InstallFlow.Data.Entities;

namespace InstallFlow.Data.Interfaces
{
    public interface ICategoryRepo
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetBySlugAsync(string slug);
        Task<Category> CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task RemoveProductAsync(int categoryId, int productId);
        Task SaveChangesAsync();
    }
}