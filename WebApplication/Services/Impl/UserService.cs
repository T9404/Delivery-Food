﻿using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication.Data;
using WebApplication.Entities;
using WebApplication.Exceptions;
using WebApplication.Mappers;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;
using WebApplication.Utils;

namespace WebApplication.Services.Impl;

public class UserService : IUserService
{
    private readonly DataBaseContext _context;
    private readonly IJwtService _jwtService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserService(IJwtProvider jwtProvider, DataBaseContext context, IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor)
    {
        _jwtProvider = jwtProvider;
        _context = context;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<RegistrationResponse> CreateUser(User user)
    {
        await CheckEmailAlreadyExists(user);
        var userHashed = CreateUserWithHashedPassword(user);
        _context.Users.Add(userHashed);
        await _context.SaveChangesAsync();
        Log.Information("User {Email} registered successfully", userHashed.Email);
        return new RegistrationResponse {FullName = userHashed.FullName, Email = userHashed.Email};
    }

    public LoginResponse Login(LoginRequest loginRequest)
    {
        var inputUser = GetUserByEmail(loginRequest.Email);
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, inputUser.Password);
        CheckIsValidPassword(isValidPassword);
        
        var accessToken = _jwtProvider.GenerateAccessToken(inputUser);
        var refreshToken = _jwtProvider.GenerateRefreshToken(inputUser);
        _jwtService.SaveRefreshToken(inputUser, refreshToken);
        Log.Information("User {Email} logged in successfully", inputUser.Email);
        return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
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
        Log.Information("User {Email} refreshed tokens successfully", user.Email);
        return new RefreshResponse {AccessToken = accessToken, RefreshToken = refreshToken};
    }
    
    public void Logout()
    {
        var email = GetMyEmail();
        var user = GetUserByEmail(email);
        _jwtService.RemoveRefreshToken(user);
        Log.Information("User {Email} logged out successfully", user.Email);
    }
    
    public UserProfileResponse GetMyProfile()
    {
        var email = GetMyEmail();
        User user = GetUserByEmail(email);
        UserProfileResponse userProfileResponse = UserMapper.EntityToUserDto(user);
        Log.Information("User {Email} profile sent successfully", user.Email);
        return userProfileResponse;
    }

    public User UpdateUser(UserEdit user)
    {
        var email = GetMyEmail();
        var userToUpdate = GetUserByEmail(email);

        userToUpdate.FullName = user.FullName;
        userToUpdate.BirthDate = user.BirthDate != default ? user.BirthDate.ToUniversalTime() : userToUpdate.BirthDate;
        userToUpdate.Address = !string.IsNullOrEmpty(user.AddressId) ? user.AddressId : userToUpdate.Address;
        userToUpdate.Gender = user.Gender != default ? user.Gender : userToUpdate.Gender;
        userToUpdate.Phone = !string.IsNullOrEmpty(user.Phone) ? user.Phone : userToUpdate.Phone;

        _context.SaveChanges();
        Log.Information("User {Email} updated successfully", userToUpdate.Email);
        return userToUpdate;
    }
    
    private async Task CheckEmailAlreadyExists(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            throw new UserAlreadyExistsException("This email is already associated with an account.");
        }
    }
    
    private User CreateUserWithHashedPassword(User user)
    {
        var userHashed = new User
        {
            FullName = user.FullName,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Email = user.Email,
            BirthDate = DateTime.SpecifyKind(user.BirthDate, DateTimeKind.Utc),
            Gender = user.Gender,
            Role = user.Role,
            Address = user.Address,
            Phone = user.Phone
        };
        return userHashed;
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
    
    private void CheckIsValidPassword(bool isValidPassword)
    {
        if (!isValidPassword)
        {
            throw new UserNotFoundException("Invalid login or password");
        }
    }
    
    private string GetMyEmail()
    {
        var email = GetMyClaimValue(ClaimTypes.Name);
        EmailUtil.CheckEmailExists(email);
        return email;
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

    private void CheckRefreshToken(RefreshRequest request, User user)
    {
        if (!_jwtProvider.IsRefreshTokenValid(user, request.RefreshToken))
        {
            throw new InvalidTokenException("Invalid refresh token");
        }
    }
}