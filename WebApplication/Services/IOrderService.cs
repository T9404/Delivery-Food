using WebApplication.Entity;

namespace WebApplication.Services;

public interface IOrderService
{
    Task<List<Order>> GetOrders();
    Task<Order> GetOrder(Guid id);
    Task<Order> CreateOrder(Order order);
    Task<Order> ConfirmOrder(Guid id);
}