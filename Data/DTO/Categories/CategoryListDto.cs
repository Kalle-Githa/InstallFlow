namespace InstallFlow.Data.DTO.Categories
{
    public class CategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string UrlSlug { get; set; } = string.Empty;
        public int ProductCount { get; set; }  // ← bara antal
    }
}
