using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Jobs;

public class CreateJobDto
{
    public int? AssignmentId { get; set; }   // valfritt — null = fristående kalkyl

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    [StringLength(2000)]
    public string? Notes { get; set; }

    public bool IsFixedPrice { get; set; }
    public decimal? FixedCustomerPrice { get; set; }
    public decimal? FixedInternalCost { get; set; }

    public string? LaborMarkupType { get; set; }   // "Percent", "FixedAmount"
    public decimal? LaborMarkupValue { get; set; }

    public string? MaterialMarkupType { get; set; }   // "Percent", "FixedAmount"
    public decimal? MaterialMarkupValue { get; set; }

    public List<CreateJobLaborRowDto> LaborRows { get; set; } = new();
    public List<CreateJobMaterialRowDto> MaterialRows { get; set; } = new();
}