using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;

namespace WebApplication.Services.Impl;

public class UserServiceImpl : UserService
{
    private readonly DataBaseContext _context;
    private readonly IJwtService _jwtService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserServiceImpl(IJwtProvider jwtProvider, DataBaseContext context, IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor)
    {
        _jwtProvider = jwtProvider;
        _context = context;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<RegistrationResponse> CreateUser(User user)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        User userHashed = new User();
        userHashed.FullName = user.FullName;
        userHashed.Password = passwordHash;
        userHashed.Email = user.Email;
        userHashed.BirthDate = DateTime.SpecifyKind(user.BirthDate, DateTimeKind.Utc);
        userHashed.Gender = user.Gender;
        userHashed.Address = user.Address;
        userHashed.Phone = user.Phone;
        _context.Users.Add(userHashed);
        await _context.SaveChangesAsync();
        return new RegistrationResponse {FullName = userHashed.FullName, Email = userHashed.Email};
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest)
    {
        User inputUser = GetUserByEmail(loginRequest.Email);

        bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, inputUser.Password);
        if (!isValidPassword)
        {
            throw new Exception("Invalid password");
        }

        var accessToken = _jwtProvider.GenerateAccessToken(inputUser);
        var refreshToken = _jwtProvider.GenerateRefreshToken(inputUser);
        _jwtService.SaveRefreshToken(inputUser, refreshToken);

        Log.Information("User {Email} logged in", inputUser.Email);
        return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
    }
    
    public async Task<RefreshResponse> Refresh(RefreshRequest request)
    {
        var email = _jwtService.GetEmailFromRefreshToken(request.RefreshToken);
        User user = await Task.Run(() => GetUserByEmail(email));
        
        if (!_jwtProvider.IsRefreshTokenValid(user, request.RefreshToken))
        {
            throw new Exception("Invalid refresh token");
        }
        
        var accessToken = _jwtProvider.GenerateAccessToken(user);
        var refreshToken = _jwtProvider.GenerateRefreshToken(user);
        _jwtService.RemoveRefreshToken(user);
        _jwtService.SaveRefreshToken(user, refreshToken);
        
        return new RefreshResponse {AccessToken = accessToken, RefreshToken = refreshToken};
    }

    private User GetUserByEmail(string username)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == username);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        return user;
    }
    
    public void Logout()
    {
        var email = GetMyEmail();
        User user = GetUserByEmail(email);
        _jwtService.RemoveRefreshToken(user);
    }
    
    private string GetMyEmail()
    {
        var username = GetMyClaimValue(ClaimTypes.Name);
        if (username == null)
        {
            throw new Exception("Username not found");
        }
        return username;
    }
    
    private string? GetMyClaimValue(string claimType)
    {
        var result = string.Empty;
        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(claimType);
        }
        return result;
    }

}