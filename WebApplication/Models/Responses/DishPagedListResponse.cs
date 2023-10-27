using WebApplication.Entities;

namespace WebApplication.Models.Responses;

public record DishPagedListResponse(List<Dish> Dishes, PageInfoResponse PageInfo)
{
    public DishPagedListResponse() : this(new List<Dish>(), new PageInfoResponse())
    {
    }
}