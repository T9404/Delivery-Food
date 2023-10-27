using WebApplication.Entities;

namespace WebApplication.Services;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    bool IsRefreshTokenValid(User user, string refreshToken);
}