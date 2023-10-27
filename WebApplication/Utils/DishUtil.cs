using WebApplication.Entities;
using WebApplication.Exceptions;

namespace WebApplication.Utils;

internal static class DishUtil
{
    public static void CheckDishExists(Dish? dish)
    {
        if (dish == null)
        {
            throw new DishNotFoundException("Dish not found");
        }
    }
}