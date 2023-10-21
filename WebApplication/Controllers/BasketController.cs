using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers;

public class BasketController : ControllerBase
{
    private readonly BasketService _basketService;
    
    public BasketController(BasketService basketService)
    {
        _basketService = basketService;
    }
    
    [HttpGet("/basket")]
    public async Task<ActionResult<Basket>> GetBasket()
    {
        return Ok(await _basketService.GetBasket());
    }
    
    [HttpPost("/basket/{dishId}")]
    public async Task<ActionResult<Basket>> AddDishToBasket(int dishId)
    {
        return Ok(await _basketService.AddDishToBasket(dishId));
    }
    
    [HttpDelete("/basket/{dishId}")]
    public async Task<ActionResult<Basket>> DeleteDishFromBasket(int dishId)
    {
        return Ok(await _basketService.DeleteDishFromBasket(dishId));
    }
}