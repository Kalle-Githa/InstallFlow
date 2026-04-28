// JobLaborRow.cs
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.Entities
{
    public class JobLaborRow
    {
        public int Id { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public decimal Hours { get; set; }
        public decimal HourlyRate { get; set; }
    }
}