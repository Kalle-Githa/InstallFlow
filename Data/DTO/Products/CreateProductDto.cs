// CreateProductDto.cs
using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Products
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Pris får inte vara negativt.")]
        public decimal DefaultPrice { get; set; }

        public UnitType Unit { get; set; } = UnitType.Piece;
        public ProductType Type { get; set; } = ProductType.Material;

        [StringLength(300)]
        public string? Image { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Produkten måste tillhöra minst en kategori.")]
        public List<int> Categories { get; set; } = null!;
    }
}