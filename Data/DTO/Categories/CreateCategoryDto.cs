using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Categories
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
    }
}