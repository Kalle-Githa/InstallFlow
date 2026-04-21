using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Cart;


public class UpdateCartItemDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Quantity måste vara minst 1.")]
    public int Quantity { get; set; }
}


