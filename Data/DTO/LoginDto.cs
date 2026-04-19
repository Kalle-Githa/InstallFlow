using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO;

public class LoginDto
{
    
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    
}