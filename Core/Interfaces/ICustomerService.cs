using InstallFlow.Data.Entities;

namespace InstallFlow.Core.Interfaces;

public interface ICustomerService
{
    Task CreateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
    Task<Customer?> GetCustomerAsync(int id);
    Task<List<Customer>> GetAllCustomersAsync();
}