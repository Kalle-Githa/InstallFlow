using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Jobs;

public class CreateJobDto
{
    public int? AssignmentId { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    [StringLength(2000)]
    public string? Notes { get; set; }

    public bool IsFixedPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Fastpris till kund får inte vara negativt.")]
    public decimal? FixedCustomerPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Fast intern kostnad får inte vara negativ.")]
    public decimal? FixedInternalCost { get; set; }

    public string? LaborMarkupType { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Pålägg för arbete får inte vara negativt.")]
    public decimal? LaborMarkupValue { get; set; }

    public string? MaterialMarkupType { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Pålägg för material får inte vara negativt.")]
    public decimal? MaterialMarkupValue { get; set; }

    public List<CreateJobLaborRowDto> LaborRows { get; set; } = new();
    public List<CreateJobMaterialRowDto> MaterialRows { get; set; } = new();
}