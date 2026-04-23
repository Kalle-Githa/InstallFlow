using InstallFlow.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Auth;

public class CreateUserDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    [MinLength(6, ErrorMessage = "Lösenord måste vara minst 6 tecken.")]
    public string Password { get; set; } = null!;

    public UserRole Role { get; set; } = UserRole.User;
}