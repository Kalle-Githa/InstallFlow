namespace InstallFlow.Data.DTO;

public class JobLaborRowDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Hours { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal RowTotal { get; set; }   // ← Hours * HourlyRate, beräknat
    
}