using WebApplication.Entity;
using WebApplication.Models.Requests;

namespace WebApplication.Services;

public interface IDishService
{
    Task<List<Dish>> GetDishes();
    Task<Dish> GetDish(Guid id);
    Task<bool> IsUserEstimateDish(Guid dishId);
    Task SetDishEstimate(Guid dishId, SetRatingDishRequest request);
}