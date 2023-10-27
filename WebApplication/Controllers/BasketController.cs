using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Entities;
using WebApplication.Services;

namespace WebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    
    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }
    
    [HttpGet, Authorize]
    public async Task<ActionResult<Basket>> GetBasket()
    {
        return Ok(await _basketService.GetBasket());
    }
    
    [HttpPost("{dishId}"), Authorize]
    public async Task<ActionResult<Basket>> AddDishToBasket(Guid dishId)
    {
        return Ok(await _basketService.AddDishToBasket(dishId));
    }
    
    [HttpDelete("{dishId}"), Authorize]
    public async Task<ActionResult<Basket>> DeleteDishFromBasket(Guid dishId)
    {
        return Ok(await _basketService.DeleteDishFromBasket(dishId));
    }
}