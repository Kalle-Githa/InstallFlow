// UpdateProductDto.cs
using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Products
{
    public class UpdateProductDto
    {
        [StringLength(200, MinimumLength = 1)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Pris får inte vara negativt.")]
        public decimal? DefaultPrice { get; set; }

        public UnitType? Unit { get; set; }

        [StringLength(300)]
        public string? Image { get; set; }

        [MinLength(1, ErrorMessage = "Produkten måste tillhöra minst en kategori.")]
        public List<int>? Categories { get; set; }
    }
}