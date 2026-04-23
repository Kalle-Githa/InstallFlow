using InstallFlow.Data.DTO.Assignments;

namespace InstallFlow.Core.Interfaces;

public interface IAssignmentService
{
    Task<List<AssignmentDto>> GetAllAssignmentsAsync();
    Task<AssignmentDto?> GetAssignmentAsync(int id);
    Task<List<AssignmentDto>> GetAllByUserIdAsync(int userId);
    Task<AssignmentDto?> CreateAssignmentAsync(CreateAssignmentDto dto, int userId);
    Task<AssignmentDto> UpdateAssignmentAsync(UpdateAssignmentDto dto, int id, int userId, bool isAdmin);
    Task DeleteAssignmentAsync(int id, int userId, bool isAdmin);



}