using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InstallFlow.Data.Repos;

public class CustomerRepo : ICustomerRepo
{
    private readonly InstallFlowDbContext _context;

    public CustomerRepo(InstallFlowDbContext context)
    {
        _context = context;
    }


    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        

        return customer;
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await GetByIdAsync(id);
        if (customer == null)
        {
            return;
        }

        _context.Customers.Remove(customer);
        
    }

    public async Task<Customer?> UpdateAsync(int id, Customer customer)
    {
        var getCustomer = await GetByIdAsync(id);
        if (getCustomer == null) 
        {
            return null;
        }
        
          _context.Customers.Update(customer);
         
         return customer;
         
    }




    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}