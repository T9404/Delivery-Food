using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Mapper;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;

namespace WebApplication.Services.Impl;

public class UserServiceImpl : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataBaseContext _context;
    private readonly IJwtService _jwtService;
    private readonly IJwtProvider _jwtProvider;
    
    public UserServiceImpl(IJwtProvider jwtProvider, DataBaseContext context, IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtProvider = jwtProvider;
        _context = context;
        _jwtService = jwtService;
    }
    
    public async Task<RegistrationResponse> CreateUser(User user)
    {
        await CheckEmailAlreadyExists(user);
        var userHashed = CreateUserWithHashedPassword(user);
        _context.Users.Add(userHashed);
        await _context.SaveChangesAsync();
        return new RegistrationResponse {FullName = userHashed.FullName, Email = userHashed.Email};
    }

    private async Task CheckEmailAlreadyExists(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            throw new Exception("This email is already associated with an account.");
        }
    }

    private static User CreateUserWithHashedPassword(User user)
    {
        var userHashed = new User
        {
            FullName = user.FullName,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Email = user.Email,
            BirthDate = DateTime.SpecifyKind(user.BirthDate, DateTimeKind.Utc),
            Gender = user.Gender,
            Address = user.Address,
            Phone = user.Phone
        };
        return userHashed;
    }

    public LoginResponse Login(LoginRequest loginRequest)
    {
        var inputUser = GetUserByEmail(loginRequest.Email);
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, inputUser.Password);
        CheckIsValidPassword(isValidPassword);
        
        var accessToken = _jwtProvider.GenerateAccessToken(inputUser);
        var refreshToken = _jwtProvider.GenerateRefreshToken(inputUser);
        _jwtService.SaveRefreshToken(inputUser, refreshToken);
        return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    private static void CheckIsValidPassword(bool isValidPassword)
    {
        if (!isValidPassword)
        {
            throw new Exception("Invalid password");
        }
    }

    public async Task<RefreshResponse> Refresh(RefreshRequest request)
    {
        var email = _jwtService.GetEmailFromRefreshToken(request.RefreshToken);
        var user = await Task.Run(() => GetUserByEmail(email));
        CheckRefreshToken(request, user);
        
        var accessToken = _jwtProvider.GenerateAccessToken(user);
        var refreshToken = _jwtProvider.GenerateRefreshToken(user);
        _jwtService.RemoveRefreshToken(user);
        _jwtService.SaveRefreshToken(user, refreshToken);
        return new RefreshResponse {AccessToken = accessToken, RefreshToken = refreshToken};
    }

    private void CheckRefreshToken(RefreshRequest request, User user)
    {
        if (!_jwtProvider.IsRefreshTokenValid(user, request.RefreshToken))
        {
            throw new Exception("Invalid refresh token");
        }
    }

    public UserProfileResponse GetMyProfile()
    {
        var email = GetMyEmail();
        User user = GetUserByEmail(email);
        UserProfileResponse userProfileResponse = UserMapper.EntityToUserDto(user);
        return userProfileResponse;
    }

    public User UpdateUser(UserEdit user)
    {
        var email = GetMyEmail();
        var userToUpdate = GetUserByEmail(email);

        userToUpdate.FullName = user.FullName ?? userToUpdate.FullName;
        userToUpdate.BirthDate = user.BirthDate != default ? user.BirthDate.ToUniversalTime() : userToUpdate.BirthDate;
        userToUpdate.Address = !string.IsNullOrEmpty(user.AddressId) ? user.AddressId : userToUpdate.Address;
        userToUpdate.Gender = user.Gender != default ? user.Gender : userToUpdate.Gender;
        userToUpdate.Phone = !string.IsNullOrEmpty(user.Phone) ? user.Phone : userToUpdate.Phone;

        _context.SaveChanges();
        return userToUpdate;
    }

    private User GetUserByEmail(string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            throw new Exception("Email not found");
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