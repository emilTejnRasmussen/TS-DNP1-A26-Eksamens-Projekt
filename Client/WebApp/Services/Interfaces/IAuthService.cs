using DTO.Auth;

namespace WebApp.Services.Interfaces;

public interface IAuthService
{
    Task LoginAsync(LoginRequest request);
    Task RegisterAsync(RegisterRequest request);
    void Logout();
}