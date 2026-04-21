using InstallFlow.Data.Enums;

namespace InstallFlow.Data.DTO.Products
{
    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal DefaultPrice { get; set; }
        public UnitType Unit { get; set; }
        public ProductType Type { get; set; }
        public string? Image { get; set; }
        public string UrlSlug { get; set; } = null!;
    }
}