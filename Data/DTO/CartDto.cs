namespace InstallFlow.Data.DTO
{
    public class CartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? AssignmentId { get; set; }
        public int? JobId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new();
        public decimal TotalPrice => CartItems.Sum(x => x.TotalPrice);
    }
}