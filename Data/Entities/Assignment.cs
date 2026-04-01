using InstallFlow.Data.Enums;

namespace InstallFlow.Data.Entities
{
    public class Assignment
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public int? UpdatedByUserId { get; set; }
        public User? UpdatedByUser { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public AssignmentStatus Status { get; set; } = AssignmentStatus.Draft;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Job> Jobs { get; set; } = new List<Job>();


    }
}
