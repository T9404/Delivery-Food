using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Entity;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;
using WebApplication.Services;

namespace WebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    
    public AuthController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        return Ok(await _userService.CreateUser(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
    {
        return Ok(await _userService.Login(loginRequest));
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> Refresh(RefreshRequest refreshRequest)
    {
        return Ok(await _userService.Refresh(refreshRequest));
    }
    
    [HttpPost("logout"), Authorize]
    public ActionResult Logout()
    {
        _userService.Logout();
        return Ok();
    }
}