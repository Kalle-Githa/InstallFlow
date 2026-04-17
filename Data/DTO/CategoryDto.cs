namespace InstallFlow.Data.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string UrlSlug { get; set; } = string.Empty;
        public List<ProductDto> Products { get; set; } = new();
    }
}