using InstallFlow.Data.Enums;

namespace InstallFlow.Data.DTO.Auth;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}