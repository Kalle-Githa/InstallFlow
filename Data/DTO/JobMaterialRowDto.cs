namespace InstallFlow.Data.DTO;

public class JobMaterialRowDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public decimal RowTotal { get; set; }   // ← Quantity * UnitPrice, beräknat
    public bool IsExtraMaterial { get; set; }
}