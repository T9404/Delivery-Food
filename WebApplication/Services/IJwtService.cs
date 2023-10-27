using WebApplication.Entities;

namespace WebApplication.Services;

public interface IJwtService
{
    void SaveRefreshToken(User user, string refreshToken);
    void RemoveRefreshToken(User user);
    string GetEmailFromRefreshToken(string? refreshToken);
}