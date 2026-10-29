using DTO.User;
using Entities;
using Exceptions;
using Mapper;
using Microsoft.AspNetCore.Identity;
using RepositoryContract;
using Services.Exceptions;
using Services.Interfaces;

namespace Services;

public class UserService(
    IUserRepository userRepo,
    IPasswordHasher<User> passwordHasher) : IUserService
{
    public async Task<UserDto> GetUserAsync(int id)
    {
        var user = await userRepo.GetSingleAsync(id);
        return UserMapper.ToDto(user);
    }

    public List<UserDto> GetMany(string? nameContains = null)
    {
        var users = userRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(nameContains))
        {
            users = users.Where(u => u.Username.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }

        var result = users
            .AsEnumerable()
            .Select(UserMapper.ToDto)
            .ToList();

        return result;
    }

    public async Task UpdateUserAsync(int userId, UpdateUserRequest request)
    {
        await EnsureUsernameAvailableAsync(userId, request);

        var userToUpdate = await userRepo.GetSingleAsync(userId);
        
        userToUpdate.Username = request.Username;

        await userRepo.UpdateAsync(userToUpdate);
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        var userToUpdate = await userRepo.GetSingleAsync(userId);

        var authResult = passwordHasher.VerifyHashedPassword(
            userToUpdate,
            userToUpdate.hashedPassword,
            request.CurrentPassword
        );

        if (authResult == PasswordVerificationResult.Failed)
        {
            throw new InvalidCurrentPasswordException();
        }

        var hashedPassword = passwordHasher.HashPassword(
            userToUpdate,
            request.NewPassword
        );

        userToUpdate.hashedPassword = hashedPassword;

        await userRepo.UpdateAsync(userToUpdate);
    }

    public async Task DeleteAsync(int id)
    {
        await userRepo.DeleteAsync(id);
    }

    private async Task EnsureUsernameAvailableAsync(int userId, UpdateUserRequest request)
    {
        var existingUser = await userRepo.GetByUsernameAsync(request.Username);

        if (existingUser is not null && existingUser.Id != userId)
        {
            throw new UsernameAlreadyExistsException(request.Username);
        }
    }
}