﻿using Microsoft.AspNetCore.Mvc;
using WebApplication.Entity;
using WebApplication.Services;

namespace WebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    
    public AuthController(IUserService _userService)
    {
        this._userService = _userService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        return Ok(await _userService.CreateUser(user));
    }
    
    
}