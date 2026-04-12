using InstallFlow.Data.Entities;

namespace InstallFlow.Data.Interfaces;

public interface IJobRepo
{
    Task<List<Job>> GetAllAsync(int? assignmentId = null);
    Task<Job?> GetByIdAsync(int id);
    Task<Job> CreateAsync(Job job);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}