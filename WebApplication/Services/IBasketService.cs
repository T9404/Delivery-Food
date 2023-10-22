namespace WebApplication.Services;

public interface IBasketService
{
    Task<Basket> GetBasket();
    Task<Basket> AddDishToBasket(int dishId);
    Task<Basket> DeleteDishFromBasket(int dishId);
}