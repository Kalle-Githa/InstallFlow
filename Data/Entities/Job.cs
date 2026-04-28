// Job.cs
using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.Entities
{
    public class Job
    {
        public int Id { get; set; }

        public int? AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }

        public int? JobTemplateId { get; set; }
        public JobTemplate? JobTemplate { get; set; }

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public int? UpdatedByUserId { get; set; }
        public User? UpdatedByUser { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public JobStatus Status { get; set; } = JobStatus.Draft;

        [MaxLength(2000)]
        public string? Notes { get; set; }

        public bool IsFixedPrice { get; set; }
        public decimal? FixedCustomerPrice { get; set; }
        public decimal? FixedInternalCost { get; set; }

        public MarkupType LaborMarkupType { get; set; } = MarkupType.Percent;
        public decimal? LaborMarkupValue { get; set; }

        public MarkupType MaterialMarkupType { get; set; } = MarkupType.Percent;
        public decimal? MaterialMarkupValue { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<JobLaborRow> LaborRows { get; set; } = new List<JobLaborRow>();
        public ICollection<JobMaterialRow> MaterialRows { get; set; } = new List<JobMaterialRow>();
    }
}