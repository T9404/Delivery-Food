using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Enums;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;

namespace WebApplication.Services.Impl;

public class DishService : IDishService
{
    private const int PageSize = 4;
    private readonly DataBaseContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public DishService(DataBaseContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<DishPagedListResponse> GetDishes(DishCategory[] categories, bool vegetarian, TypeSorting sorting, int page)
    {
        var dishes = _context.Dishes.AsQueryable();
        
        dishes = FilterCategories(categories, dishes);
        dishes = FilterVegetarian(vegetarian, dishes);
        dishes = SortDishes(sorting, dishes);
        dishes = GetPage(page, dishes);
        
        var result = new DishPagedListResponse(await dishes.ToListAsync(), new PageInfoResponse(PageSize, await _context.Dishes.CountAsync(), page));
        return result;
    }

    private static IQueryable<Dish> SortDishes(TypeSorting sorting, IQueryable<Dish> inputDishes)
    {
        var dishes = inputDishes;
        dishes = sorting switch
        {
            TypeSorting.PriceAsc => dishes.OrderBy(d => d.Price),
            TypeSorting.PriceDesc => dishes.OrderByDescending(d => d.Price),
            TypeSorting.RatingAsc => dishes.OrderBy(d => d.Rating),
            TypeSorting.RatingDesc => dishes.OrderByDescending(d => d.Rating),
            TypeSorting.NameAsc => dishes.OrderBy(d => d.Name),
            TypeSorting.NameDesc => dishes.OrderByDescending(d => d.Name),
            _ => dishes
        };
        return dishes;
    }
    
    private static IQueryable<Dish> FilterVegetarian(bool vegetarian, IQueryable<Dish> inputDishes)
    {
        var dishes = inputDishes;
        if (vegetarian)
        {
            dishes = dishes.Where(d => d.Vegetarian);
        }
        return dishes;
    }
    
    private static IQueryable<Dish> FilterCategories(DishCategory[] categories, IQueryable<Dish> inputDishes)
    {
        var dishes = inputDishes;
        if (categories.Length != 0)
        {
            var categoryStrings = categories.Select(c => c.ToString()).ToArray();
            dishes = dishes.Where(d => categoryStrings.Contains(d.Category));
        }
        return dishes;
    }
    
    private static IQueryable<Dish> GetPage(int page, IQueryable<Dish> inputDishes)
    {
        var dishes = inputDishes;
        dishes = dishes.Skip(page * PageSize).Take(PageSize);
        return dishes;
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
        if (dish == null)
        {
            throw new Exception("Dish not found");
        }
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