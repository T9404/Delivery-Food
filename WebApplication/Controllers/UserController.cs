using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Entities;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;
using WebApplication.Services;

namespace WebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet, Authorize]
    public ActionResult<UserProfileResponse> GetMyProfile()
    {
        return Ok(_userService.GetMyProfile());
    }
    
    [HttpPut, Authorize]
    public ActionResult<UserProfileResponse> UpdateUser(UserEdit user)
    {
        return Ok(_userService.UpdateUser(user));
    }
}