namespace InstallFlow.Data.DTO
{
    public class CategorySummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string UrlSlug { get; set; } = null!;
    }
}