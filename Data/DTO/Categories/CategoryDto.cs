using InstallFlow.Data.DTO.Products;

namespace InstallFlow.Data.DTO.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string UrlSlug { get; set; } = string.Empty;
        public List<ProductSummaryDto> Products { get; set; } = [];
    }
}