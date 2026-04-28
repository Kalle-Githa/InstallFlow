// JobTemplate.cs
using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.Entities
{
    public class JobTemplate
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string JobType { get; set; } = null!;

        public bool IsFixedPrice { get; set; }
        public decimal? FixedCustomerPrice { get; set; }
        public decimal? FixedInternalCost { get; set; }

        public MarkupType LaborMarkupType { get; set; } = MarkupType.None;
        public decimal? LaborMarkupValue { get; set; }

        public MarkupType MaterialMarkupType { get; set; } = MarkupType.None;
        public decimal? MaterialMarkupValue { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<JobTemplateLaborRow> LaborRows { get; set; } = new List<JobTemplateLaborRow>();
        public ICollection<JobTemplateMaterialRow> MaterialRows { get; set; } = new List<JobTemplateMaterialRow>();
    }
}