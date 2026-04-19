using InstallFlow.Data.DTO;

namespace InstallFlow.Core.Interfaces;

public interface IAssignmentService
{
    Task<List<AssignmentDto>> GetAllAssignmentsAsync();
    Task<AssignmentDto?> GetAssignmentAsync(int id);
    Task<List<AssignmentDto>> GetAllByUserIdAsync(int userId);
    Task<AssignmentDto?> CreateAssignmentAsync(CreateAssignmentDto dto,int userId);
    Task<AssignmentDto?> UpdateAssignmentAsync(UpdateAssignmentDto dto, int id,int userId);
    Task<bool> DeleteAssignmentAsync(int id);
}