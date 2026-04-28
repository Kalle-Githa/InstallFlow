// Product.cs
using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public decimal DefaultPrice { get; set; }
        public UnitType Unit { get; set; } = UnitType.Piece;
        public ProductType Type { get; set; } = ProductType.Material;

        [MaxLength(300)]
        public string? Image { get; set; }

        [Required]
        [MaxLength(200)]
        public string UrlSlug { get; set; } = null!;

        public bool IsActive { get; set; }

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public int? UpdatedByUserId { get; set; }
        public User? UpdatedByUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}