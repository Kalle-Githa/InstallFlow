// UpdateCategoryDto.cs
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Categories
{
    public class UpdateCategoryDto
    {
        [StringLength(200, MinimumLength = 1)]
        public string? Name { get; set; }

        [StringLength(300)]
        public string? Image { get; set; }
    }
}