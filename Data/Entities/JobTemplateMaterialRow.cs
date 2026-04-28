// JobTemplateMaterialRow.cs
using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.Entities
{
    public class JobTemplateMaterialRow
    {
        public int Id { get; set; }

        public int JobTemplateId { get; set; }
        public JobTemplate JobTemplate { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public decimal DefaultQuantity { get; set; }
        public UnitType Unit { get; set; } = UnitType.Piece;
        public decimal DefaultUnitPrice { get; set; }

        public bool IsOptional { get; set; }
        public bool IsExtraMaterial { get; set; }
    }
}