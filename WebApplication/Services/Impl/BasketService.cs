using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Entities;
using WebApplication.Utils;

namespace WebApplication.Services.Impl;

public class BasketService : IBasketService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataBaseContext _context;
    
    public BasketService(IHttpContextAccessor httpContextAccessor, DataBaseContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    
    public async Task<Basket> GetBasket()
    {
        var userEmail = GetMyEmail();
        var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserEmail == userEmail);
        
        if (basket == null)
        {
            basket = new Basket();
            basket.UserEmail = userEmail;
            basket.TotalPrice = 0;
            basket.Dishes = new List<Guid>();
            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();
        }
        return basket;
    }
    
    public async Task<Basket> AddDishToBasket(Guid dishId)
    {
        var basket = await GetBasket();
        var dish = GetDish(dishId);
        
        basket.Dishes.Add(dish.Id);
        basket.TotalPrice += dish.Price;
        await _context.SaveChangesAsync();
        return basket;
    }
    
    public async Task<Basket> DeleteDishFromBasket(Guid dishId)
    {
        var basket = await GetBasket();
        var dish = GetDish(dishId);
        
        basket.Dishes.Remove(dish.Id);
        basket.TotalPrice -= dish.Price;
        await _context.SaveChangesAsync();
        return basket;
    }
    
    private string GetMyEmail()
    {
        var email = GetMyClaimValue(ClaimTypes.Name);
        EmailUtil.CheckEmailExists(email);
        return email;
    }
    
    private string? GetMyClaimValue(string claimType)
    {
        var result = string.Empty;
        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(claimType);
        }
        return result;
    }
    
    private Dish GetDish(Guid dishId)
    {
        var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
        DishUtil.CheckDishExists(dish);
        return dish;
    }
}
