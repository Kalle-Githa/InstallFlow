using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;

namespace InstallFlow.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepo _customerRepo;

    public CustomerService(ICustomerRepo customerRepo)
    {
        _customerRepo = customerRepo;
    }



    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            CompanyName = dto.Company,
            ContactPerson = dto.Person,
            Email = dto.Email,
            Phone = dto.Phone,
            OrganizationNumber = dto.OrganizationNumber

        };
        await _customerRepo.CreateAsync(customer);






        return new CustomerDto
        {
            Id = customer.Id,
            Company = customer.CompanyName,
            Person = customer.ContactPerson,
            Email = customer.Email,
            Phone = customer.Phone,
            OrganizationNumber = customer.OrganizationNumber,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
        };
    }

    public async Task<List<CustomerDto>> GetAllCustomersAsync()
    {
        var allCustomers = await _customerRepo.GetAllAsync();
        return allCustomers.Select(customer => new CustomerDto
        {
            Id = customer.Id,
            Company = customer.CompanyName,
            Person = customer.ContactPerson,
            Email = customer.Email,
            Phone = customer.Phone,
            OrganizationNumber = customer.OrganizationNumber,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        }).ToList();
    }

    public async Task<CustomerDto?> GetCustomerAsync(int id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);

        if (customer == null)
        {
            return null;

        }

        return new CustomerDto
        {

            Id = customer.Id,
            Company = customer.CompanyName,
            Person = customer.ContactPerson,
            Email = customer.Email,
            Phone = customer.Phone,
            OrganizationNumber = customer.OrganizationNumber,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt

        };

    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null)
        {
            return false;
        }

        await _customerRepo.DeleteAsync(id);

        return true;
    }

    public async Task<CustomerDto?> UpdateCustomerAsync(UpdateCustomerDto dto, int id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);

        if (customer == null)
        {
            return null;
        }


        if (dto.Company != null)
            customer.CompanyName = dto.Company;
        if (dto.Person != null)
            customer.ContactPerson = dto.Person;
        if (dto.Email != null)
            customer.Email = dto.Email;
        if (dto.Phone != null)
            customer.Phone = dto.Phone;
        if (dto.OrganizationNumber != null)
            customer.OrganizationNumber = dto.OrganizationNumber;

        customer.UpdatedAt = DateTime.Now;

        await _customerRepo.UpdateAsync(id, customer);


        return new CustomerDto
        {
            Id = id,
            Company = customer.CompanyName,
            Person = customer.ContactPerson,
            Email = customer.Email,
            Phone = customer.Phone,
            OrganizationNumber = customer.OrganizationNumber,
            UpdatedAt = customer.UpdatedAt
        };

    }
}