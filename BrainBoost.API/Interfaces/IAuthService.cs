using BrainBoost.API.DTOs.Auth;

namespace BrainBoost.API.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> GetMeAsync(int userId);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}