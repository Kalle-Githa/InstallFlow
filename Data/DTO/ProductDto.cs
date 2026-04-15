using InstallFlow.Data.Enums;

namespace InstallFlow.Data.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal DefaultPrice { get; set; }
        public UnitType Unit { get; set; } = UnitType.Piece;
        public string? Image { get; set; }
        public string UrlSlug { get; set; } = null!;

        public List<CategorySummaryDto> Categories { get; set; } = [];
    }
}