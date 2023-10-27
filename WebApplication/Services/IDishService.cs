using WebApplication.Entities;
using WebApplication.Enums;
using WebApplication.Models.Requests;
using WebApplication.Models.Responses;

namespace WebApplication.Services;

public interface IDishService
{
    Task<DishPagedListResponse> GetDishes(DishCategory[] categories, bool vegetarian, TypeSorting sorting, int page);
    Task<Dish> GetDish(Guid id);
    Task<bool> IsUserEstimateDish(Guid dishId);
    Task SetDishEstimate(Guid dishId, SetRatingDishRequest request);
}