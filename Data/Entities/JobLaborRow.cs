namespace InstallFlow.Data.Entities
{
    public class JobLaborRow
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Hours { get; set; }
        public decimal HourlyRate { get; set; }
    }
}
