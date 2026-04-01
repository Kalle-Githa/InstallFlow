namespace InstallFlow.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public ICollection<Assignment> CreatedAssignments { get; set; } = new List<Assignment>();
        public ICollection<Assignment> UpdatedAssignments { get; set; } = new List<Assignment>();

        public ICollection<Job> CreatedJobs { get; set; } = new List<Job>();
        public ICollection<Job> UpdatedJobs { get; set; } = new List<Job>();
    }
}
