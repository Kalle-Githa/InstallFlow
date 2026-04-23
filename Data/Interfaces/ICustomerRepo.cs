using InstallFlow.Data.Entities;

namespace InstallFlow.Data.Interfaces;

public interface ICustomerRepo
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer> CreateAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();



}