using EmocineSveikataServer.Dto;
using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<bool> UserExists(string username);
        string CreateToken(User user);
    }
}
