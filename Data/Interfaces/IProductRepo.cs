using InstallFlow.Data.Entities;
using InstallFlow.Data.Enums;

namespace InstallFlow.Data.Interfaces
{
    public interface IProductRepo
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetByTypeAsync(ProductType type);
        Task<List<Product>> GetAllAsync(int page, int pageSize);
        Task<Product> CreateAsync(Product product);
        Task<Product?> GetBySlugAsync(string slug);
        Task<IEnumerable<Product>> SearchAsync(string searchTerm, int page, int pageSize);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();



    }
}
