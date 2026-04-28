// User.cs
using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; }

        public ICollection<Assignment> CreatedAssignments { get; set; } = new List<Assignment>();
        public ICollection<Job> CreatedJobs { get; set; } = new List<Job>();
        public ICollection<Customer> CreatedCustomers { get; set; } = new List<Customer>();
        public ICollection<Product> CreatedProducts { get; set; } = new List<Product>();
    }
}