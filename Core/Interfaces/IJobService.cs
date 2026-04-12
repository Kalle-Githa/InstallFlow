using InstallFlow.Data.DTO;

namespace InstallFlow.Core.Interfaces;

public interface IJobService
{
    Task<List<JobDto>> GetAllJobsAsync(int? assignmentId = null);
    Task<JobDto?> GetJobAsync(int id);
    Task<JobDto?> CreateJobAsync(CreateJobDto dto);
    Task<JobDto?> UpdateJobAsync(UpdateJobDto dto, int id);
    Task<bool> DeleteJobAsync(int id);
}