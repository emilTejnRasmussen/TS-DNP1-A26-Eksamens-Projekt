using DTO.User;

namespace WebApp.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserAsync(int id);
    Task<List<UserDto>> GetUsersAsync(string? nameContains = null);
    Task UpdateUserAsync(UpdateUserRequest request);
    Task ChangePasswordAsync(ChangePasswordRequest request);
    Task DeleteUserAsync();
}