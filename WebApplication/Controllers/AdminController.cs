using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Entities;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;
using WebApplication.Services;

namespace WebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IDishService _dishService;
    
    public AdminController(IDishService dishService)
    {
        _dishService = dishService;
    }
    
    [HttpPost("createDish")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<Dish>> CreateDish(DishRequest request)
    {
        return Ok(await _dishService.CreateDish(request));
    }
    
    [HttpPut("updateDish/{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<DefaultResponse>> UpdateDish(Guid id, DishRequest request)
    {
        return Ok(await _dishService.UpdateDish(id, request));
    }
    
    [HttpDelete("deleteDish/{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<DefaultResponse>> DeleteDish(Guid id)
    {
        return Ok(await _dishService.DeleteDish(id));
    }
}