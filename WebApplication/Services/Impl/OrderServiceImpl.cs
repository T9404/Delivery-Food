using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
        Log.Information("Orders sent successfully");
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
        Log.Information("Order with this GUID {} sent successfully", id);
        return order;
    }
    
    public async Task<Order> CreateOrder(OrderCreateRequest order)
    {
        var userEmail = GetMyEmail();
        var basket = await GetBasket(userEmail);
        var newOrder = await InitNewOrder(order, userEmail, basket);
        _context.Orders.Add(newOrder);
        _context.Baskets.Remove(basket);
        await _context.SaveChangesAsync();
        return newOrder;
    }

    private async Task<Order> InitNewOrder(OrderCreateRequest order, string userEmail, Basket? basket)
    {
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
        Log.Information("Order with this GUID {} created successfully", newOrder.Id);
        return newOrder;
    }

    private async Task<Basket?> GetBasket(string userEmail)
    {
        var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserEmail == userEmail);
        CheckBasketNotFound(basket);
        CheckBasketEmpty(basket);
        return basket;
    }

    private static void CheckBasketNotFound(Basket? basket)
    {
        if (basket == null)
        {
            throw new Exception("Basket not found");
        }
    }

    private static void CheckBasketEmpty(Basket basket)
    {
        if (basket.Dishes.Count == 0)
        {
            throw new Exception("Basket is empty");
        }
    }

    public async Task<Order> ConfirmOrder(Guid id)
    {
        var order = await GetDeliveredOrder(id);
        order.Status = OrderStatus.Delivered;
        await _context.SaveChangesAsync();
        Log.Error("Order with this GUID {} confirmed successfully", id);
        return order;
    }
    
    private async Task<Order> GetDeliveredOrder(Guid id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        CheckOrderNotFound(order);
        CheckOrderIsDelivered(order);
        CheckBelongingOrder(order);
        return order;
    }

    private void CheckBelongingOrder(Order order)
    {
        if (order.UserEmail != GetMyEmail())
        {
            throw new Exception("You can't confirm this order");
        }
    }

    private static void CheckOrderIsDelivered(Order order)
    {
        if (order.Status == OrderStatus.Delivered)
        {
            throw new Exception("Order already delivered");
        }
    }

    private static void CheckOrderNotFound(Order? order)
    {
        if (order == null)
        {
            throw new Exception("Order not found");
        }
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