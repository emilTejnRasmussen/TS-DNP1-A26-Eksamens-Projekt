using Entities;

namespace Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}