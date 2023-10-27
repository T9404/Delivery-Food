using WebApplication.Constants;
using WebApplication.Data;
using WebApplication.Entities;
using WebApplication.Exceptions;

namespace WebApplication.Services.Impl;

public class JwtService: IJwtService
{
    private readonly DataBaseContext _context;
    private readonly Tokens _tokens;
    
    public JwtService(DataBaseContext context, Tokens tokens)
    {
        _context = context;
        _tokens = tokens;
    }

    public void SaveRefreshToken(User user, string refreshToken)
    {
        var token = new RefreshToken
        {
            Token = refreshToken,
            Email = user.Email,
            Expires = DateTime.UtcNow.AddMinutes(_tokens.RefreshTokenExpireMinutes)
        };
        _context.RefreshTokens.Add(token);
        _context.SaveChanges();
    }
    
    public void RemoveRefreshToken(User user)
    {
        List<RefreshToken> tokens = _context.RefreshTokens.Where(t => t.Email == user.Email).ToList();
        if (tokens.Count != 0)
        {
            _context.RefreshTokens.RemoveRange(tokens);
            _context.SaveChanges();
        }
    }
    
    public string GetEmailFromRefreshToken(string refreshToken)
    {
        var token = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
        CheckFoundRefreshToken(token);
        return token.Email;
    }

    private void CheckFoundRefreshToken(RefreshToken? refreshToken)
    {
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow)
        {
            throw new InvalidTokenException("Invalid refresh token");
        }
    }
}