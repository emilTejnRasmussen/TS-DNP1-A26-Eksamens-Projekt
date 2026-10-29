using DTO.User;

namespace Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserAsync(int id);

    List<UserDto> GetMany(string? nameContains = null);
    
    Task UpdateUserAsync(int userId, UpdateUserRequest request);
    
    Task ChangePasswordAsync(int userId, ChangePasswordRequest request);
    Task DeleteAsync(int id);
}