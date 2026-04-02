using InstallFlow.Data.Enums;

namespace InstallFlow.Data.Entities
{
    public class Job
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;

        public int? JobTemplateId { get; set; }
        public JobTemplate? JobTemplate { get; set; }

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public int? UpdatedByUserId { get; set; }
        public User? UpdatedByUser { get; set; }

        public string Name { get; set; } = null!;
        public JobStatus Status { get; set; } = JobStatus.Draft;
        public string? Notes { get; set; }

        public bool IsFixedPrice { get; set; }
        public decimal? FixedCustomerPrice { get; set; }
        public decimal? FixedInternalCost { get; set; }

        public MarkupType LaborMarkupType { get; set; } = MarkupType.None;
        public decimal? LaborMarkupValue { get; set; }

        public MarkupType MaterialMarkupType { get; set; } = MarkupType.None;
        public decimal? MaterialMarkupValue { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<JobLaborRow> LaborRows { get; set; } = new List<JobLaborRow>();
        public ICollection<JobMaterialRow> MaterialRows { get; set; } = new List<JobMaterialRow>();




    }
}
