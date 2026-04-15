using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? DefaultPrice { get; set; }
        public UnitType? Unit { get; set; }
        public string? Image { get; set; }

        [MinLength(1, ErrorMessage = "Produkten måste tillhöra minst en kategori.")]
        public List<int>? Categories { get; set; }
    }
}