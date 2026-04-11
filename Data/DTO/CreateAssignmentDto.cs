namespace InstallFlow.Data.DTO;

using System.ComponentModel.DataAnnotations;



    public class CreateAssignmentDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }
    }
