using InstallFlow.Data.DTO;

namespace InstallFlow.Core.Interfaces;

public interface IAssignmentService
{
    Task<List<AssignmentDto>> GetAllAssignmentsAsync();
    Task<AssignmentDto?> GetAssignmentAsync(int id);
    Task<AssignmentDto?> CreateAssignmentAsync(CreateAssignmentDto dto);
    Task<AssignmentDto?> UpdateAssignmentAsync(UpdateAssignmentDto dto, int id);
    Task<bool> DeleteAssignmentAsync(int id);
}