using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Enums;
using WebApplication.Models.Requests;

namespace WebApplication.Services.Impl;

public class DishService : IDishService
{
    private readonly DataBaseContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public DishService(DataBaseContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<List<Dish>> GetDishes()
    {
        return await _context.Dishes.ToListAsync();
    }
    
    public async Task<Dish> GetDish(Guid id)
    {
        var dish = await _context.Dishes.FindAsync(id);
        if (dish == null)
        {
            throw new Exception("Dish not found");
        }

        return dish;
    }
    
    public Task<bool> IsUserEstimateDish(Guid dishId)
    {
        var email = GetMyEmail();
        var orders = _context.Orders.Where(o => o.UserEmail == email && o.Status == OrderStatus.Delivered).ToList();
        var dishes = orders.Select(o => o.Dishes).ToList().SelectMany(d => d).ToList();
        return Task.FromResult(dishes.Contains(dishId));
        
    }
    
    public Task SetDishEstimate(Guid dishId, SetRatingDishRequest request)
    {
        var email = GetMyEmail();
        var orders = _context.Orders.Where(o => o.UserEmail == email && o.Status == OrderStatus.Delivered).ToList();
        var dishes = orders.Select(o => o.Dishes).ToList().SelectMany(d => d).ToList();
        if (!dishes.Contains(dishId))
        {
            throw new Exception("You can't estimate this dish");
        }
        var dish = _context.Dishes.Find(dishId);
        dish.Rating = (request.Rating + dish.Rating) / 2;
        _context.Dishes.Update(dish);
        _context.SaveChanges();
        return Task.CompletedTask;
    }
    
    private string GetMyEmail()
    {
        var username = GetMyClaimValue(ClaimTypes.Name);
        if (username == null)
        {
            throw new Exception("Username not found");
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