using InstallFlow.Data.Enums;

namespace InstallFlow.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int? AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        public int? JobId { get; set; }
        public Job? Job { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public CartStatus Status { get; set; } = CartStatus.Active;
    }
}