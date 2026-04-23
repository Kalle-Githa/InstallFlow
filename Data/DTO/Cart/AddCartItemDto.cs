using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Cart;

public class AddCartItemDto
{

    public int ProductId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Antal måste vara minst 1.")]
    public int Quantity { get; set; }
}