using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Exceptions;
using WebApplication.Mapper;
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
        // if email already exists then throw exception
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            throw new UserAlreadyExistsException("User with this email already exists");
        }
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
            throw new InvalidCredentialException("Invalid password");
        }

        var accessToken = _jwtProvider.GenerateAccessToken(inputUser);
        var refreshToken = _jwtProvider.GenerateRefreshToken(inputUser);
        _jwtService.SaveRefreshToken(inputUser, refreshToken);

        return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
    }
    
    public async Task<RefreshResponse> Refresh(RefreshRequest request)
    {
        var email = _jwtService.GetEmailFromRefreshToken(request.RefreshToken);
        User user = await Task.Run(() => GetUserByEmail(email));
        
        if (!_jwtProvider.IsRefreshTokenValid(user, request.RefreshToken))
        {
            throw new TokenNotValidException("Invalid refresh token");
        }
        
        var accessToken = _jwtProvider.GenerateAccessToken(user);
        var refreshToken = _jwtProvider.GenerateRefreshToken(user);
        _jwtService.RemoveRefreshToken(user);
        _jwtService.SaveRefreshToken(user, refreshToken);
        
        return new RefreshResponse {AccessToken = accessToken, RefreshToken = refreshToken};
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

        if (user.FullName != null)
        {
            userToUpdate.FullName = user.FullName;
        }

        if (user.BirthDate != default)
        {
            userToUpdate.BirthDate = user.BirthDate.ToUniversalTime();
        }

        if (!string.IsNullOrEmpty(user.AddressId))
        {
            userToUpdate.Address = user.AddressId;
        }

        if (user.Gender != default)
        {
            userToUpdate.Gender = user.Gender;
        }

        if (!string.IsNullOrEmpty(user.Phone))
        {
            userToUpdate.Phone = user.Phone;
        }

        _context.SaveChanges();
        return userToUpdate;
    }

    private User GetUserByEmail(string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            throw new UserNotFoundException("Email not found");
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
            throw new UserNotFoundException("User with this email not found");
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