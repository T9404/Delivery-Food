using WebApplication.Entities;
using WebApplication.Models.Requests;

namespace WebApplication.Services;

public interface IOrderService
{
    Task<List<Order>> GetOrders();
    Task<Order> GetOrder(Guid id);
    Task<Order> CreateOrder(OrderCreateRequest order);
    Task<Order> ConfirmOrder(Guid id);
}