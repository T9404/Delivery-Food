using WebApplication.Enums;

namespace WebApplication.Models.Requests;

public record DishRequest(string Name, string Description, int Price, string Image, bool Vegetarian, DishCategory Category)
{
    public DishRequest() : this(string.Empty, string.Empty, 0, string.Empty,
        false, DishCategory.Wok)
    {
    }
}