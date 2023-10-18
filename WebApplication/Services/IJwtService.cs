using WebApplication.Entity;

namespace WebApplication.Services;

public interface IJwtService
{
    void SaveRefreshToken(User user, string refreshToken);
    void SetRevokedRefreshToken(string refreshToken);
    void RemoveRefreshToken(User user);
    string GetUsernameFromRefreshToken(string refreshToken);
}