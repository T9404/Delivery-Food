using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Models.Responses;

namespace WebApplication.Services.Impl;

public class UserService : IUserService
{
    private readonly DataBaseContext _context;
    
    public UserService(DataBaseContext context)
    {
        _context = context;
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
}