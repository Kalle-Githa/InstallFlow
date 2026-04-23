using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InstallFlow.Data.Repos;

public class UserRepo : IUserRepo
{
    private readonly InstallFlowDbContext _context;

    public UserRepo(InstallFlowDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        return user;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}