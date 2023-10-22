using WebApplication.Entity;

namespace WebApplication.Services;

public interface IBasketService
{
    Task<Basket> GetBasket();
    Task<Basket> AddDishToBasket(Guid dishId);
    Task<Basket> DeleteDishFromBasket(Guid dishId);
}
