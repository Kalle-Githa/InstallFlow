// JobTemplateLaborRow.cs
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.Entities
{
    public class JobTemplateLaborRow
    {
        public int Id { get; set; }

        public int JobTemplateId { get; set; }
        public JobTemplate JobTemplate { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public decimal DefaultHours { get; set; }
        public decimal DefaultHourlyRate { get; set; }
    }
}