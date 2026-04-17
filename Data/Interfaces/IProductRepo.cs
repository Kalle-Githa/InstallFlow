using InstallFlow.Data.Entities;

namespace InstallFlow.Data.Interfaces
{
    public interface IProductRepo
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task<Product> CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);



    }
}
