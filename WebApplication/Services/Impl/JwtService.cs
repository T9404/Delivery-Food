using WebApplication.Constants;
using WebApplication.Data;
using WebApplication.Entity;
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
    
    public string GetEmailFromRefreshToken(string? refreshToken)
    {
        if (refreshToken == null)
        {
            throw new Exception("Invalid refresh token");
        }
        
        var token = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
        
        if (token == null)
        {
            throw new TokenNotValidException("Invalid refresh token");
        }
        
        return token.Email;
    }
}