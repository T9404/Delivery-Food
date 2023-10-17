using WebApplication.Entity;
using WebApplication.Models.Responses;

namespace WebApplication.Services;

public interface IUserService
{
    Task<RegistrationResponse> CreateUser(User user);
}