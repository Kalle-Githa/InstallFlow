using InstallFlow.Data.DTO.Auth;

namespace InstallFlow.Core.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAsync(string username, string password);
    Task<UserDto> CreateUserAsync(CreateUserDto dto);
}