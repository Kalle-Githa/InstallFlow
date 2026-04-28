// CreateCategoryDto.cs
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Categories
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [StringLength(300)]
        public string? Image { get; set; }
    }
}