using InstallFlow.Data.Entities;

namespace InstallFlow.Data.Interfaces;

public interface IUserRepo
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task SaveChangesAsync();
}