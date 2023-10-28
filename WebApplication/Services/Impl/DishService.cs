using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication.Data;
using WebApplication.Entities;
using WebApplication.Enums;
using WebApplication.Exceptions;
using WebApplication.Models.Requests;
using WebApplication.Utils;
using WebApplication.Models.Responses;

namespace WebApplication.Services.Impl;

public class DishService : IDishService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataBaseContext _context;
    private int pageSize;
    
    public DishService(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor, DataBaseContext context)
    {
        pageSize = _configuration.GetValue<int>("AppSettings:PageSize");
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    
    public async Task<DishPagedListResponse> GetDishes(DishCategory[] categories, bool vegetarian, TypeSorting sorting, int page)
    {
        var dishes = _context.Dishes.AsQueryable();
        dishes = ApplyCategoryFilter(categories, dishes);
        dishes = ApplyVegetarianFilter(vegetarian, dishes);
        
        int countPage = await CalculateCountPage(dishes);
        
        dishes = ApplySorting(sorting, dishes);
        dishes = ApplyPagination(page, dishes);
        
        var result = 
            new DishPagedListResponse(await dishes.ToListAsync(), 
                new PageInfoResponse(pageSize, countPage, page));
        Log.Information("Dishes sent successfully, page {Page}, categories {Categories}, " +
                        "vegetarian {Vegetarian}", page, categories.ToString(), vegetarian);
        return result;
    }
    
    public async Task<Dish> GetDish(Guid id)
    {
        var dish = await _context.Dishes.FindAsync(id);
        DishUtil.CheckDishExists(dish);
        Log.Information("Dish {Name} sent successfully", dish.Name);
        return dish;
    }

    public Task<bool> IsUserEstimateDish(Guid dishId)
    {
        var email = GetMyEmail();
        var orders = _context.Orders.Where(o => o.UserEmail == email && o.Status == OrderStatus.Delivered).ToList();
        var dishes = orders.Select(o => o.Dishes).ToList().SelectMany(d => d).ToList();
        return Task.FromResult(dishes.Contains(dishId));
    }
    
    public async Task SetDishEstimate(Guid dishId, SetRatingDishRequest request)
    {
        var email = GetMyEmail();
        var orders = _context.Orders.Where(o => o.UserEmail == email && o.Status == OrderStatus.Delivered).ToList();
        var dishes = orders.Select(o => o.Dishes).ToList().SelectMany(d => d).ToList();

        CheckPossibilityEstimating(dishId, dishes);
        
        var dish = await GetDish(dishId);
        dish.CountRatings++;
        dish.Rating = ((dish.Rating * (dish.CountRatings - 1)) + request.Rating) / (dish.CountRatings);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Dish> CreateDish(DishRequest request)
    {
        var dish = new Dish
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category.ToString(),
            Vegetarian = request.Vegetarian,
            Image = request.Image,
            Rating = 0,
            CountRatings = 0
        };
        await _context.Dishes.AddAsync(dish);
        await _context.SaveChangesAsync();
        Log.Information("Dish {Name} created successfully", dish.Name);
        return dish;
    }
    
    public async Task<DefaultResponse> UpdateDish(Guid id, DishRequest request)
    {
        var dish = await _context.Dishes.FindAsync(id);
        DishUtil.CheckDishExists(dish);
        UpdateDish(dish, request);
        _context.Dishes.Update(dish);
        await _context.SaveChangesAsync();
        Log.Information("Dish {Name} updated successfully", dish.Name);
        return new DefaultResponse("Dish updated successfully");
    }
    
    public async Task<DefaultResponse> DeleteDish(Guid id)
    {
        var dish = await GetDish(id);
        DishUtil.CheckDishExists(dish);
        _context.Dishes.Remove(dish);
        await _context.SaveChangesAsync();
        Log.Information("Dish {Name} deleted successfully", dish.Name);
        return new DefaultResponse("Dish deleted successfully");
    }
    
    private IQueryable<Dish> ApplyCategoryFilter(DishCategory[] categories, IQueryable<Dish> inputDishes)
    {
        var dishes = inputDishes;
        if (categories.Length != 0)
        {
            var categoryStrings = categories.Select(c => c.ToString()).ToArray();
            dishes = dishes.Where(d => categoryStrings.Contains(d.Category));
        }
        return dishes;
    }
    
    private IQueryable<Dish> ApplyVegetarianFilter(bool vegetarian, IQueryable<Dish> inputDishes)
    {
        var dishes = inputDishes;
        if (vegetarian)
        {
            dishes = dishes.Where(d => d.Vegetarian);
        }
        return dishes;
    }
    
    private async Task<int> CalculateCountPage(IQueryable<Dish> dishes)
    {
        int countDishes = await dishes.CountAsync();
        int countAnswer = countDishes / pageSize;
        return countAnswer;
    }

    private IQueryable<Dish> ApplySorting(TypeSorting sorting, IQueryable<Dish> inputDishes)
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
    
    private IQueryable<Dish> ApplyPagination(int page, IQueryable<Dish> inputDishes)
    {
        var dishes = inputDishes;
        dishes = dishes.Skip(page * pageSize).Take(pageSize);
        return dishes;
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

    private void CheckPossibilityEstimating(Guid dishId, ICollection<Guid> dishes)
    {
        EnsureDishEstimationAllowed(dishId, dishes);
        
        var dish = _context.Dishes.Find(dishId);
        DishUtil.CheckDishExists(dish);
        _context.Dishes.Update(dish);
        _context.SaveChanges();
        Log.Information("Dish {Name} estimated successfully", dish.Name);
    }

    private static void EnsureDishEstimationAllowed(Guid dishId, ICollection<Guid> dishes)
    {
        if (!dishes.Contains(dishId))
        {
            throw new DishEstimationException("You can't estimate this dish");
        }
    }
    
    private void UpdateDish(Dish dish, DishRequest request)
    {
        dish.Name = request.Name;
        dish.Description = request.Description;
        dish.Price = request.Price; 
        dish.Category = request.Category.ToString();
        dish.Vegetarian = request.Vegetarian;
        dish.Image = request.Image;
    }
}