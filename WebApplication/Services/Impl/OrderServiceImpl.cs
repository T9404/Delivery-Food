using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Enums;
using WebApplication.Models.Requests;

namespace WebApplication.Services.Impl;

public class OrderServiceImpl : IOrderService
{
    private readonly DataBaseContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public OrderServiceImpl(DataBaseContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<List<Order>> GetOrders()
    {
        var userEmail = GetMyEmail();
        var orders = await _context.Orders.Where(o => o.UserEmail == userEmail).ToListAsync();
        return orders;
    }
    
    public async Task<Order> GetOrder(Guid id)
    {
        var userEmail = GetMyEmail();
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserEmail == userEmail);
        if (order == null)
        {
            throw new Exception("Order not found");
        }
        return order;
    }
    
    public async Task<Order> CreateOrder(OrderCreateRequest order)
    {
        var userEmail = GetMyEmail();
        var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserEmail == userEmail);
        if (basket == null)
        {
            throw new Exception("Basket not found");
        }
        
        if (basket.Dishes.Count == 0)
        {
            throw new Exception("Basket is empty");
        }
        
        var newOrder = new Order
        {
            UserEmail = userEmail,
            Dishes = basket.Dishes,
            Price = basket.TotalPrice,
            Status = OrderStatus.InProcess,
            Address = order.AddressId,
            DeliveryTime = order.DeliveryTime.ToUniversalTime()
        };
        
        _context.Orders.Add(newOrder);
        _context.Baskets.Remove(basket);
        await _context.SaveChangesAsync();
        return newOrder;
    }
    
    public async Task<Order> ConfirmOrder(Guid id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            throw new Exception("Order not found");
        }
        if (order.Status == OrderStatus.Delivered)
        {
            throw new Exception("Order already delivered");
        }
        
        if (order.UserEmail != GetMyEmail())
        {
            throw new Exception("You can't confirm this order");
        }

        order.Status = OrderStatus.Delivered;
        await _context.SaveChangesAsync();
        return order;
    }
    
    private string GetMyEmail()
    {
        var username = GetMyClaimValue(ClaimTypes.Name);
        if (username == null)
        {
            throw new Exception("Username not found");
        }
        return username;
    }
    
    private string? GetMyClaimValue(string claimType)
    {
        var result = string.Empty;
        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(claimType);
        }
        return result;
    }
}