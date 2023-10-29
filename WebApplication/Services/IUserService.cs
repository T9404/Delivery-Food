using WebApplication.Entities;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;

namespace WebApplication.Services;

public interface IUserService
{
    Task<RegistrationResponse> CreateUser(User user);
    LoginResponse Login(LoginRequest loginRequest);
    UserProfileResponse GetMyProfile();
    UserProfileResponse UpdateUser(UserEdit user);
    Task<RefreshResponse> Refresh(RefreshRequest refreshRequest);
    void Logout();
}