using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal DefaultPrice { get; set; }
        public UnitType Unit { get; set; } = UnitType.Piece;
        public string? Image { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Produkten måste tillhöra minst en kategori.")]
        public List<int> Categories { get; set; } = null!;
    }
}
