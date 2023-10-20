using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Entity;

namespace WebApplication.Services.Impl;

public class DishService : IDishService
{
    private readonly DataBaseContext _context;
    
    public DishService(DataBaseContext context)
    {
        _context = context;
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
        throw new NotImplementedException();
    }
    
    public Task SetDishEstimate(Guid dishId, int estimate)
    {
        throw new NotImplementedException();
    }
}