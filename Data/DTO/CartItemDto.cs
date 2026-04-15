namespace InstallFlow.Data.DTO
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPriceSnapshot { get; set; }
        public decimal TotalPrice => Quantity * UnitPriceSnapshot;
    }
}