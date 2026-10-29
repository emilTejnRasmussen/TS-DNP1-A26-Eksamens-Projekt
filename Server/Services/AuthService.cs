using DTO.Auth;
using DTO.User;
using Entities;
using Exceptions;
using Mapper;
using Microsoft.AspNetCore.Identity;
using RepositoryContract;
using Services.Exceptions;
using Services.Interfaces;

namespace Services;

public class AuthService(
    IUserRepository userRepo,
    IPasswordHasher<User> passwordHasher) : IAuthService
{ 
    public async Task<UserDto> RegisterAsync(RegisterRequest request)
    {
        EnsureUsernameDoNotExist(request.Username);

        var user = new User(request.Username);

        user.hashedPassword = passwordHasher.HashPassword(user, request.Password);

        var createdUser = await userRepo.AddAsync(user);

        return UserMapper.ToDto(createdUser);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await userRepo.GetByUsernameAsync(request.Username);

        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        var authResult = passwordHasher.VerifyHashedPassword(
            user,
            user.hashedPassword,
            request.Password
        );

        if (authResult == PasswordVerificationResult.Failed)
        {
            throw new InvalidCredentialsException();
        }
        
        
    }
    
    private async Task EnsureUsernameDoNotExist(string username)
    {
        var usernameAlreadyExists = await userRepo.UsernameAlreadyExistsAsync(username);

        if (usernameAlreadyExists)
        {
            throw new UsernameAlreadyExistsException(username);
        }
    }
}