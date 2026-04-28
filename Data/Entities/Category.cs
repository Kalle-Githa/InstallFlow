// Category.cs
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(300)]
        public string? Image { get; set; }

        [Required]
        [MaxLength(200)]
        public string UrlSlug { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}