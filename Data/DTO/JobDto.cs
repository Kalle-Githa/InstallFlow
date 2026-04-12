namespace InstallFlow.Data.DTO;

public class JobDto
{
    public int Id { get; set; }
    public int? AssignmentId { get; set; }
    public string? AssignmentName { get; set; }
    public string Name { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }

    public bool IsFixedPrice { get; set; }
    public decimal? FixedCustomerPrice { get; set; }
    public decimal? FixedInternalCost { get; set; }

    public string LaborMarkupType { get; set; } = null!;
    public decimal? LaborMarkupValue { get; set; }
    public string MaterialMarkupType { get; set; } = null!;
    public decimal? MaterialMarkupValue { get; set; }

    public List<JobLaborRowDto> LaborRows { get; set; } = new();
    public List<JobMaterialRowDto> MaterialRows { get; set; } = new();

    // ===== Beräknade totaler (skickas direkt till klienten) =====
    public decimal LaborCost { get; set; }
    public decimal MaterialCost { get; set; }
    public decimal InternalCost { get; set; }
    public decimal CustomerPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}