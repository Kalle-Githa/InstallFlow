using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Jobs;

public class CreateJobLaborRowDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Range(0.001, double.MaxValue, ErrorMessage = "Timmar måste vara större än 0.")]
    public decimal Hours { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Timtaxa måste vara större än 0.")]
    public decimal HourlyRate { get; set; }
}