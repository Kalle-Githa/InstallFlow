// Customer.cs
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = null!;

        [MaxLength(200)]
        public string? ContactPerson { get; set; }

        [MaxLength(200)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(20)]
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