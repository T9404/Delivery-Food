using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Entity;
using WebApplication.Services;

namespace WebApplication.Controllers;

public class UserController : ControllerBase
{
    private readonly UserService userService;
    
    public UserController(UserService userService)
    {
        this.userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        return Ok(await userService.GetUsers());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        return Ok(await userService.GetUser(id));
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        return Ok(await userService.CreateUser(user));
    }
    
    [HttpPut]
    public async Task<ActionResult<User>> UpdateUser(UserEditModel user)
    {
        return Ok(await userService.UpdateUser(user));
    }
    
    [HttpGet, Authorize]
    public ActionResult<string> GetUserProfile()
    {
        return Ok(userService.GetMyProfile());
    }
}