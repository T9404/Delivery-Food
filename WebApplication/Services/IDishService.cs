using WebApplication.Entity;

namespace WebApplication.Services;

public interface IDishService
{
    Task<List<Dish>> GetDishes();
    Task<Dish> GetDish(Guid id);
    Task<bool> IsUserEstimateDish(Guid dishId);
    Task SetDishEstimate(Guid dishId, int estimate);
}