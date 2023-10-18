using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebApplication.Constants;
using WebApplication.Data;
using WebApplication.Entity;

namespace WebApplication.Services.Impl;

public class JwtProvider : IJwtProvider
{
    private readonly DataBaseContext _context;
    private readonly Tokens _tokens;
    
    public JwtProvider(DataBaseContext context,  Tokens tokens)
    {
        _context = context;
        _tokens = tokens;
    }
    
    public string GenerateAccessToken(User user)
    {
        return CreateToken(user, _tokens.AccessTokenKey, _tokens.AccessTokenExpireMinutes);
    }
    
    public string GenerateRefreshToken(User user)
    {
        return CreateToken(user, _tokens.RefreshTokenKey, _tokens.RefreshTokenExpireMinutes);
    }
    
    private string CreateToken(User user, SecurityKey key, int expireMinutes)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Email),
        };

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            audience: "apiAudience",
            issuer: "apiIssuer",
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireMinutes),
            signingCredentials: cred
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool IsRefreshTokenValid(User user, string refreshToken)
    {
        var token = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
        if (IsTokenNull(token))
        {
            return false;
        }
        CheckTokenUsername(token, user);
        CheckTokenRevoked(token);
        CheckTokenExpired(token);   
        return true;
    }
    
    private static bool IsTokenNull(RefreshToken? token)
    {
        return token == null;
    }
    
    private static void CheckTokenUsername(RefreshToken token, User user)
    {
        if (token.Email != user.Email)
        {
            throw new Exception("Invalid refresh token");
        }
    }
    
    private static void CheckTokenRevoked(RefreshToken token)
    {
        if (token.Revoked != null)
        {
            throw new Exception("Invalid refresh token");
        }
    }
    
    private static void CheckTokenExpired(RefreshToken token)
    {
        if (token.Expires < DateTime.UtcNow)
        {
            throw new Exception("Invalid refresh token");
        }
    }
}