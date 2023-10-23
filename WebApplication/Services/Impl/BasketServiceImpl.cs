using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Exceptions;

namespace WebApplication.Services.Impl;

public class BasketServiceImpl : IBasketService
{
    private readonly DataBaseContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public BasketServiceImpl(DataBaseContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
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
        var userEmail = GetMyEmail();
        var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserEmail == userEmail);
        if (dish == null)
        {
            throw new DishNotFoundException("Dish not found");
        }
        if (basket == null)
        {
            basket = new Basket();
            basket.UserEmail = userEmail;
            basket.TotalPrice = 0;
            basket.Dishes = new List<Guid>();
            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();
        }
        basket.Dishes.Add(dish.Id);
        basket.TotalPrice += dish.Price;
        await _context.SaveChangesAsync();
        return basket;
    }
    
    public async Task<Basket> DeleteDishFromBasket(Guid dishId)
    {
        var userEmail = GetMyEmail();
        var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserEmail == userEmail);
        var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish == null)
        {
            throw new DishNotFoundException("Dish not found");
        }
        if (basket == null)
        {
            basket = new Basket();
            basket.UserEmail = userEmail;
            basket.TotalPrice = 0;
            basket.Dishes = new List<Guid>();
            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();
        }
        basket.Dishes.Remove(dish.Id);
        basket.TotalPrice -= dish.Price;
        await _context.SaveChangesAsync();
        return basket;
    }
    
    private string GetMyEmail()
    {
        var username = GetMyClaimValue(ClaimTypes.Name);
        if (username == null)
        {
            throw new UserNotFoundException("User with this email not found");
        }
        return username;
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
}
