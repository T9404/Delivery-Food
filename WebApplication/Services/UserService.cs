using WebApplication.Entity;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;

namespace WebApplication.Services;

public interface UserService
{
    Task<RegistrationResponse> CreateUser(User user);
    Task<LoginResponse> Login(LoginRequest loginRequest);
    UserProfileResponse GetMyProfile();
    User UpdateUser(UserEdit user);
    Task<RefreshResponse> Refresh(RefreshRequest refreshRequest);
    void Logout();
}