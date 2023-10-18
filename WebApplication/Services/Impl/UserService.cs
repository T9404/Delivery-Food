using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;

namespace WebApplication.Services.Impl;

public class UserService : IUserService
{
    private readonly DataBaseContext _context;
    private readonly IJwtService _jwtService;
    private readonly IJwtProvider _jwtProvider;

    
    public UserService(IJwtProvider jwtProvider, DataBaseContext context, IJwtService jwtService)
    {
        _jwtProvider = jwtProvider;
        _context = context;
        _jwtService = jwtService;
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

    public async Task<DefaultResponse> Login(LoginRequest loginRequest)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(loginRequest.Password);
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        if (BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            return new DefaultResponse {Description = "Login success"};
        }
        
        throw new Exception("Password is incorrect");
    }
}