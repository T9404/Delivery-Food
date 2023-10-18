using WebApplication.Entity;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;

namespace WebApplication.Services;

public interface IUserService
{
    Task<RegistrationResponse> CreateUser(User user);
    Task<LoginResponse> Login(LoginRequest loginRequest);
    void Logout();
}