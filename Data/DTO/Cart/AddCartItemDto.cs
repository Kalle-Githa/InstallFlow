namespace InstallFlow.Data.DTO.Cart;

public class AddCartItemDto
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}