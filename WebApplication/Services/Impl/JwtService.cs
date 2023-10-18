using WebApplication.Constants;
using WebApplication.Data;
using WebApplication.Entity;

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
        RefreshToken token = new RefreshToken();
        token.Token = refreshToken;
        token.Email = user.Email;
        token.Expires = DateTime.UtcNow.AddMinutes(_tokens.RefreshTokenExpireMinutes);
        _context.RefreshTokens.Add(token);
        _context.SaveChanges();
    }
    
    public void SetRevokedRefreshToken(string refreshToken)
    {
        RefreshToken? token = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
        if (token != null)
        {
            token.Revoked = DateTime.UtcNow;
            _context.RefreshTokens.Update(token);
            _context.SaveChanges();
        }
    }
    
    public void RemoveRefreshToken(User user)
    {
        RefreshToken? token = _context.RefreshTokens.FirstOrDefault(t => t.Email == user.Email);
        if (token != null)
        {
            token.Revoked = DateTime.UtcNow;
            _context.RefreshTokens.Update(token);
            _context.SaveChanges();
        }
    }
    
    public string GetUsernameFromRefreshToken(string refreshToken)
    {
        RefreshToken? token = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
        if (token == null)
        {
            throw new Exception("Invalid refresh token");
        }
        
        return token.Email;
    }
}