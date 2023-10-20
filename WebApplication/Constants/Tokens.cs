using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication.Constants;

public class Tokens
{
    public SecurityKey AccessTokenKey { get; set; }
    public SecurityKey RefreshTokenKey { get; set; }
    public int AccessTokenExpireMinutes { get; set; }
    public int RefreshTokenExpireMinutes { get; set; }
    private readonly IConfiguration _configuration;
    
    public Tokens(IConfiguration configuration)
    {
        _configuration = configuration;
        InitKeys();
        InitExpiredTimes();
    }

    private void InitKeys()
    {
        AccessTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:AccessToken").Value!));
        RefreshTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:RefreshToken").Value!));
    }
    
    private void InitExpiredTimes()
    {
        AccessTokenExpireMinutes = int.Parse(_configuration.GetSection("AppSettings:AccessTokenExpireMinutes").Value!);
        RefreshTokenExpireMinutes = int.Parse(_configuration.GetSection("AppSettings:RefreshTokenExpireMinutes").Value!);
    }
}