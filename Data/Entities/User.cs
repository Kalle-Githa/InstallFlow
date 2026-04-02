using InstallFlow.Data.Enums;

namespace InstallFlow.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; }

        public ICollection<Assignment> CreatedAssignments { get; set; } = new List<Assignment>();
        public ICollection<Job> CreatedJobs { get; set; } = new List<Job>();
    }
}

