using InstallFlow.Data.Enums;

namespace InstallFlow.Data.Entities
{
    public class JobTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string JobType { get; set; } = null!;

        public bool IsFixedPrice { get; set; }
        public decimal? FixedCustomerPrice { get; set; }
        public decimal? FixedInternalCost { get; set; }

        public MarkupType LaborMarkupType { get; set; } = MarkupType.Percent;
        public decimal? LaborMarkupValue { get; set; }

        public MarkupType MaterialMarkupType { get; set; } = MarkupType.Percent;
        public decimal? MaterialMarkupValue { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<JobTemplateLaborRow> LaborRows { get; set; } = new List<JobTemplateLaborRow>();
        public ICollection<JobTemplateMaterialRow> MaterialRows { get; set; } = new List<JobTemplateMaterialRow>();
    }
}
