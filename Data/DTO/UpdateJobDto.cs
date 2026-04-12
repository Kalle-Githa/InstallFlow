namespace InstallFlow.Data.DTO;

using System.ComponentModel.DataAnnotations;

public class UpdateJobDto
{
    [StringLength(200, MinimumLength = 1)]
    public string? Name { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public string? Status { get; set; }
    public bool? IsFixedPrice { get; set; }
    public decimal? FixedCustomerPrice { get; set; }
    public decimal? FixedInternalCost { get; set; }

    public string? LaborMarkupType { get; set; }
    public decimal? LaborMarkupValue { get; set; }
    public string? MaterialMarkupType { get; set; }
    public decimal? MaterialMarkupValue { get; set; }
}