using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Enums;
using InstallFlow.Data.Interfaces;

namespace InstallFlow.Core.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepo _assignmentRepo;
    private readonly ICustomerRepo _customerRepo;

    public AssignmentService(IAssignmentRepo assignmentRepo, ICustomerRepo customerRepo)
    {
        _assignmentRepo = assignmentRepo;
        _customerRepo = customerRepo;
    }

    public async Task<List<AssignmentDto>> GetAllAssignmentsAsync()
    {
        var assignments = await _assignmentRepo.GetAllAsync();
        return assignments.Select(MapToDto).ToList();
    }

    public async Task<AssignmentDto?> GetAssignmentAsync(int id)
    {
        var assignment = await _assignmentRepo.GetByIdAsync(id);
        if (assignment == null) return null;
        return MapToDto(assignment);
    }

    public async Task<AssignmentDto?> CreateAssignmentAsync(CreateAssignmentDto dto)
    {
        // Kolla att kunden finns innan vi skapar uppdraget
        var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
        if (customer == null) return null;

        var assignment = new Assignment
        {
            CustomerId = dto.CustomerId,
            Name = dto.Name,
            Description = dto.Description,
            Address = dto.Address,
            Status = AssignmentStatus.Draft,
            CreatedByUserId = 1,                 // TODO: ersätt med inloggad user när JWT är klar
            CreatedAt = DateTime.Now
        };

        await _assignmentRepo.CreateAsync(assignment);
        await _assignmentRepo.SaveChangesAsync();

        // Hämta om med Include så vi får med Customer för mappningen
        var created = await _assignmentRepo.GetByIdAsync(assignment.Id);
        return MapToDto(created!);
    }

    public async Task<AssignmentDto?> UpdateAssignmentAsync(UpdateAssignmentDto dto, int id)
    {
        var assignment = await _assignmentRepo.GetByIdAsync(id);
        if (assignment == null) return null;

        if (dto.Name != null)
            assignment.Name = dto.Name;
        if (dto.Description != null)
            assignment.Description = dto.Description;
        if (dto.Address != null)
            assignment.Address = dto.Address;
        if (dto.Status != null && Enum.TryParse<AssignmentStatus>(dto.Status, out var parsed))
            assignment.Status = parsed;

        assignment.UpdatedAt = DateTime.Now;
        assignment.UpdatedByUserId = 1;          // TODO: JWT

        await _assignmentRepo.SaveChangesAsync();
        return MapToDto(assignment);
    }

    public async Task<bool> DeleteAssignmentAsync(int id)
    {
        var assignment = await _assignmentRepo.GetByIdAsync(id);
        if (assignment == null) return false;

        await _assignmentRepo.DeleteAsync(id);
        await _assignmentRepo.SaveChangesAsync();
        return true;
    }

    // ===== Privat hjälpmetod för att slippa duplicera mapping-koden =====
    private static AssignmentDto MapToDto(Assignment a)
    {
        return new AssignmentDto
        {
            Id = a.Id,
            CustomerId = a.CustomerId,
            CustomerName = a.Customer.CompanyName,
            Name = a.Name,
            Description = a.Description,
            Address = a.Address,
            Status = a.Status.ToString(),
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        };
    }
}