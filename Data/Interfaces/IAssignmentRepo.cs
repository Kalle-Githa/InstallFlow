using InstallFlow.Data.Entities;

namespace InstallFlow.Data.Interfaces;

public interface IAssignmentRepo
{
    Task<List<Assignment>> GetAllAsync();
    Task<List<Assignment>> GetAllByUserIdAsync(int userId);
    Task<Assignment?> GetByIdAsync(int id);
    Task<Assignment> CreateAsync(Assignment assignment);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}