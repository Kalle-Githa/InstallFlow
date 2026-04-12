namespace InstallFlow.Data.Entities
{
    public class JobTemplateLaborRow
    {
        public int Id { get; set; }

        public int JobTemplateId { get; set; }
        public JobTemplate JobTemplate { get; set; } = null!;

        public string Name { get; set; } = null!;
        public decimal DefaultHours { get; set; }
        public decimal DefaultHourlyRate { get; set; }
 

    }
}
