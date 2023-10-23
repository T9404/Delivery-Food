using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Entity;
using WebApplication.Enums;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;
using WebApplication.Services;

namespace WebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;
    
    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }
    
    [HttpGet]
    public async Task<ActionResult<DishPagedListResponse>> GetDishes([FromQuery] DishCategory[] categories, 
        bool vegetarian, TypeSorting sorting, int page)
    {
        return Ok(await _dishService.GetDishes(categories, vegetarian, sorting, page));
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Dish>> GetDish(Guid id)
    {
        return Ok(await _dishService.GetDish(id));
    }
    
    [HttpGet("{dishId}/rating/check"), Authorize]
    public async Task<ActionResult<Boolean>> IsUserEstimateDish(Guid dishId)
    {
        return Ok(await _dishService.IsUserEstimateDish(dishId));
    }
    
    [HttpPost("{dishId}/rating"), Authorize]
    public async Task<ActionResult> SetDishEstimate(Guid dishId, SetRatingDishRequest request)
    {
        await _dishService.SetDishEstimate(dishId, request);
        return Ok();
    }
}
