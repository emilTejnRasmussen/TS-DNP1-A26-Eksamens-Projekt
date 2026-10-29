using DTO.Auth;
using Entities;
using Exceptions;
using Microsoft.AspNetCore.Identity;
using RepositoryContract;
using Services.Exceptions;
using Services.Interfaces;

namespace Services;

public class AuthService(
    IUserRepository userRepo,
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenService) : IAuthService
{ 
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        await EnsureUsernameDoesNotExistAsync(request.Username);

        var user = new User(request.Username);

        user.hashedPassword = passwordHasher.HashPassword(user, request.Password);

        var createdUser = await userRepo.AddAsync(user);

        var token = tokenService.GenerateToken(createdUser);

        return new AuthResponse(token);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await userRepo.GetByUsernameAsync(request.Username)
                   ?? throw new InvalidCredentialsException();

        EnsureValidPassword(user, request.Password);

        var token = tokenService.GenerateToken(user);

        return new AuthResponse(token);
    }

    private void EnsureValidPassword(User user, string password)
    {
        var result = passwordHasher.VerifyHashedPassword(
            user,
            user.hashedPassword,
            password
        );

        if (result == PasswordVerificationResult.Failed)
        {
            throw new InvalidCredentialsException();
        }
    }
    
    private async Task EnsureUsernameDoesNotExistAsync(string username)
    {
        var usernameAlreadyExists = await userRepo.UsernameAlreadyExistsAsync(username);

        if (usernameAlreadyExists)
        {
            throw new UsernameAlreadyExistsException(username);
        }
    }
}