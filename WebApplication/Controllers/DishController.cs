using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<Dish>> GetDish(int id)
    {
        return Ok(await _dishService.GetDish(id));
    }
    
    [HttpGet]
    public async Task<ActionResult<Boolean>> IsUserEstimateDish(int dishId)
    {
        return Ok(await _dishService.IsUserEstimateDish(dishId));
    }
    
    [HttpPost]
    public async void setDishEstimate(int dishId, int estimate)
    {
        await _dishService.SetDishEstimate(dishId, estimate);
    }
}