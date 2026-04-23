using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Jobs;

public class CreateJobMaterialRowDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Range(0.001, double.MaxValue, ErrorMessage = "Antal måste vara större än 0.")]
    public decimal Quantity { get; set; }

    public string Unit { get; set; } = "Piece";

    [Range(0, double.MaxValue, ErrorMessage = "Enhetspris får inte vara negativt.")]
    public decimal UnitPrice { get; set; }

    public bool IsExtraMaterial { get; set; }
}