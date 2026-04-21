namespace InstallFlow.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? OrganizationNumber { get; set; }
        public int CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }
        public int? UpdatedByUserId { get; set; }
        public User? UpdatedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
