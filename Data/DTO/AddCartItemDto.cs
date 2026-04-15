namespace InstallFlow.Data.DTO;

public class AddCartItemDto
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}