using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Enums;
using InstallFlow.Data.Interfaces;

namespace InstallFlow.Core.Services;

public class JobService : IJobService
{
    private readonly IJobRepo _jobRepo;
    private readonly IAssignmentRepo _assignmentRepo;

    public JobService(IJobRepo jobRepo, IAssignmentRepo assignmentRepo)
    {
        _jobRepo = jobRepo;
        _assignmentRepo = assignmentRepo;
    }

    public async Task<List<JobDto>> GetAllJobsAsync(int? assignmentId = null)
    {
        var jobs = await _jobRepo.GetAllAsync(assignmentId);
        return jobs.Select(MapToDto).ToList();
    }

    public async Task<JobDto?> GetJobAsync(int id)
    {
        var job = await _jobRepo.GetByIdAsync(id);
        if (job == null) return null;
        return MapToDto(job);
    }

    public async Task<JobDto?> CreateJobAsync(CreateJobDto dto)
    {
        // Om assignmentId är satt, kolla att det faktiskt finns
        if (dto.AssignmentId.HasValue)
        {
            var assignment = await _assignmentRepo.GetByIdAsync(dto.AssignmentId.Value);
            if (assignment == null) return null;
        }

        var job = new Job
        {
            AssignmentId = dto.AssignmentId,
            Name = dto.Name,
            Notes = dto.Notes,
            Status = JobStatus.Draft,
            IsFixedPrice = dto.IsFixedPrice,
            FixedCustomerPrice = dto.FixedCustomerPrice,
            FixedInternalCost = dto.FixedInternalCost,
            LaborMarkupType = ParseMarkupType(dto.LaborMarkupType),
            LaborMarkupValue = dto.LaborMarkupValue,
            MaterialMarkupType = ParseMarkupType(dto.MaterialMarkupType),
            MaterialMarkupValue = dto.MaterialMarkupValue,
            CreatedByUserId = 2,   // TODO: JWT
            CreatedAt = DateTime.Now,
            LaborRows = dto.LaborRows.Select(r => new JobLaborRow
            {
                Name = r.Name,
                Hours = r.Hours,
                HourlyRate = r.HourlyRate,

            }).ToList(),
            MaterialRows = dto.MaterialRows.Select(r => new JobMaterialRow
            {
                Name = r.Name,
                Quantity = r.Quantity,
                Unit = ParseUnitType(r.Unit),
                UnitPrice = r.UnitPrice,

                IsExtraMaterial = r.IsExtraMaterial
            }).ToList()
        };

        await _jobRepo.CreateAsync(job);
        await _jobRepo.SaveChangesAsync();

        // Hämta om med alla includes så mapping blir komplett
        var created = await _jobRepo.GetByIdAsync(job.Id);
        return MapToDto(created!);
    }

    public async Task<JobDto?> UpdateJobAsync(UpdateJobDto dto, int id)
    {
        var job = await _jobRepo.GetByIdAsync(id);
        if (job == null) return null;

        if (dto.Name != null) job.Name = dto.Name;
        if (dto.Notes != null) job.Notes = dto.Notes;
        if (dto.IsFixedPrice.HasValue) job.IsFixedPrice = dto.IsFixedPrice.Value;
        if (dto.FixedCustomerPrice.HasValue) job.FixedCustomerPrice = dto.FixedCustomerPrice;
        if (dto.FixedInternalCost.HasValue) job.FixedInternalCost = dto.FixedInternalCost;
        if (dto.LaborMarkupType != null) job.LaborMarkupType = ParseMarkupType(dto.LaborMarkupType);
        if (dto.LaborMarkupValue.HasValue) job.LaborMarkupValue = dto.LaborMarkupValue;
        if (dto.MaterialMarkupType != null) job.MaterialMarkupType = ParseMarkupType(dto.MaterialMarkupType);
        if (dto.MaterialMarkupValue.HasValue) job.MaterialMarkupValue = dto.MaterialMarkupValue;

        if (dto.Status != null && Enum.TryParse<JobStatus>(dto.Status, out var parsedStatus))
            job.Status = parsedStatus;

        job.UpdatedAt = DateTime.Now;
        job.UpdatedByUserId = 2;   // TODO: JWT

        await _jobRepo.SaveChangesAsync();
        return MapToDto(job);
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        var job = await _jobRepo.GetByIdAsync(id);
        if (job == null) return false;

        await _jobRepo.DeleteAsync(id);
        await _jobRepo.SaveChangesAsync();
        return true;
    }

    // ===== Privat mapping + beräkningar =====
    private static JobDto MapToDto(Job j)
    {
        // 1. Räkna ut varje rads totaler
        var laborRows = j.LaborRows
            .Select(r => new JobLaborRowDto
            {
                Id = r.Id,
                Name = r.Name,
                Hours = r.Hours,
                HourlyRate = r.HourlyRate,
                RowTotal = r.Hours * r.HourlyRate,
            }).ToList();

        var materialRows = j.MaterialRows
            .Select(r => new JobMaterialRowDto
            {
                Id = r.Id,
                Name = r.Name,
                Quantity = r.Quantity,
                Unit = r.Unit.ToString(),
                UnitPrice = r.UnitPrice,
                RowTotal = r.Quantity * r.UnitPrice,
                IsExtraMaterial = r.IsExtraMaterial
            }).ToList();

        // 2. Räkna ut interna kostnader (innan pålägg)
        var laborCostRaw = laborRows.Sum(r => r.RowTotal);
        var materialCostRaw = materialRows.Sum(r => r.RowTotal);

        // 3. Applicera pålägg
        var laborCostWithMarkup = ApplyMarkup(laborCostRaw, j.LaborMarkupType, j.LaborMarkupValue);
        var materialCostWithMarkup = ApplyMarkup(materialCostRaw, j.MaterialMarkupType, j.MaterialMarkupValue);

        // 4. Hantera fastpris-läget
        decimal internalCost;
        decimal customerPrice;

        if (j.IsFixedPrice)
        {
            internalCost = j.FixedInternalCost ?? 0;
            customerPrice = j.FixedCustomerPrice ?? 0;
        }
        else
        {
            internalCost = laborCostRaw + materialCostRaw;
            customerPrice = laborCostWithMarkup + materialCostWithMarkup;
        }

        return new JobDto
        {
            Id = j.Id,
            AssignmentId = j.AssignmentId,
            AssignmentName = j.Assignment?.Name,
            Name = j.Name,
            Status = j.Status.ToString(),
            Notes = j.Notes,
            IsFixedPrice = j.IsFixedPrice,
            FixedCustomerPrice = j.FixedCustomerPrice,
            FixedInternalCost = j.FixedInternalCost,
            LaborMarkupType = j.LaborMarkupType.ToString(),
            LaborMarkupValue = j.LaborMarkupValue,
            MaterialMarkupType = j.MaterialMarkupType.ToString(),
            MaterialMarkupValue = j.MaterialMarkupValue,
            LaborRows = laborRows,
            MaterialRows = materialRows,
            LaborCost = laborCostRaw,
            MaterialCost = materialCostRaw,
            InternalCost = internalCost,
            CustomerPrice = customerPrice,
            CreatedAt = j.CreatedAt,
            UpdatedAt = j.UpdatedAt
        };
    }

    private static decimal ApplyMarkup(decimal baseAmount, MarkupType type, decimal? value)
    {
        if (value == null) return baseAmount;

        return type switch
        {
            MarkupType.Percent => baseAmount * (1 + value.Value / 100m),
            MarkupType.FixedAmount => baseAmount + value.Value,
            _ => baseAmount
        };
    }

    private static MarkupType ParseMarkupType(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return MarkupType.None;
        return Enum.TryParse<MarkupType>(input, true, out var result) ? result : MarkupType.None;
    }

    private static UnitType ParseUnitType(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return UnitType.Piece;
        return Enum.TryParse<UnitType>(input, true, out var result) ? result : UnitType.Piece;
    }
}