using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Jobs;

public class CreateJobLaborRowDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    public decimal Hours { get; set; }
    public decimal HourlyRate { get; set; }
  
}