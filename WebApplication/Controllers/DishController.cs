using Microsoft.AspNetCore.Mvc;
using WebApplication.Entity;
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
    public async Task<ActionResult<List<Dish>>> GetDishes()
    {
        return Ok(await _dishService.GetDishes());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Dish>> GetDish(Guid id)
    {
        return Ok(await _dishService.GetDish(id));
    }
    
    [HttpGet("{dishId}")]
    public async Task<ActionResult<Boolean>> IsUserEstimateDish(Guid dishId)
    {
        return Ok(await _dishService.IsUserEstimateDish(dishId));
    }
    
    /*[HttpPost]
    public async void setDishEstimate(Guid dishId, int estimate)
    {
        await _dishService.SetDishEstimate(dishId, estimate);
    }*/
}
