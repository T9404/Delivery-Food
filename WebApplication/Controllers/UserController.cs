using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Entity;
using WebApplication.Models.Requests;
using WebApplication.Services;

namespace WebApplication.Controllers;

public class UserController : ControllerBase
{
    private readonly UserService userService;
    
    public UserController(UserService userService)
    {
        this.userService = userService;
    }
    
    [HttpGet, Authorize]
    public async Task<ActionResult<User>> GetMyProfile()
    {
        return Ok(await userService.GetMyProfile());
    }
    
    [HttpPut]
    public async Task<ActionResult<User>> UpdateUser(UserEdit user)
    {
        return Ok(await userService.UpdateUser(user));
    }
}