using InstallFlow.Data.DTO.Customers;

namespace InstallFlow.Core.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto customer, int userId);
    Task<CustomerDto?> GetCustomerAsync(int id);
    Task<List<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto> UpdateCustomerAsync(UpdateCustomerDto dto, int id, int useId, bool isAdmin);
    Task DeleteCustomerAsync(int id);
}