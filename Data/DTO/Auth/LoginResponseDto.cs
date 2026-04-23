namespace InstallFlow.Data.DTO.Auth;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
}