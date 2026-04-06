using InstallFlow.Data.DTO;

namespace InstallFlow.Core.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto customer);
    Task DeleteCustomerAsync(int id);
    Task<CustomerDto?> GetCustomerAsync(int id);
    Task<List<CustomerDto>> GetAllCustomersAsync();
}