using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Jobs;

public class CreateJobMaterialRowDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    public decimal Quantity { get; set; }
    public string Unit { get; set; } = "Piece";   // "Piece", "Meter", etc.
    public decimal UnitPrice { get; set; }
    public bool IsExtraMaterial { get; set; }
}