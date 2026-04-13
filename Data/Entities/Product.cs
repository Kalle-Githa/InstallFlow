using InstallFlow.Data.Enums;

namespace InstallFlow.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal DefaultPrice { get; set; }
        public UnitType Unit { get; set; } = UnitType.Piece;
        public string? Image { get; set; }
        public string UrlSlug { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();




    }
}
