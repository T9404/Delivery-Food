using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Entity;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;
using WebApplication.Services;

namespace WebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    
    public UserController(IUserService userService)
    {
        this.userService = userService;
    }
    
    [HttpGet, Authorize]
    public ActionResult<UserProfileResponse> GetMyProfile()
    {
        return Ok(userService.GetMyProfile());
    }
    
    [HttpPut, Authorize]
    public ActionResult<User> UpdateUser(UserEdit user)
    {
        return Ok(userService.UpdateUser(user));
    }
}