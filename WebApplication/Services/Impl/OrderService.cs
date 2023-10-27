using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication.Data;
using WebApplication.Entities;
using WebApplication.Enums;
using WebApplication.Exceptions;
using WebApplication.Models.Requests;
using WebApplication.Utils;

namespace WebApplication.Services.Impl;

public class OrderService : IOrderService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataBaseContext _context;
    
    public OrderService(IHttpContextAccessor httpContextAccessor, DataBaseContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
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
        CheckOrderNotFound(order);
        Log.Information("Order with this GUID {} find successfully", id);
        return order;
    }
    
    public async Task<Order> CreateOrder(OrderCreateRequest order)
    {
        var userEmail = GetMyEmail();
        var basket = await GetBasket(userEmail);
        var newOrder = InitNewOrder(order, userEmail, basket);
        _context.Orders.Add(newOrder);
        _context.Baskets.Remove(basket);
        await _context.SaveChangesAsync();
        Log.Information("Order with this GUID {} created successfully", newOrder.Id);
        return newOrder;
    }
    
    public async Task<Order> ConfirmOrder(Guid id)
    {
        var order = await GetDeliveredOrder(id);
        order.Status = OrderStatus.Delivered;
        await _context.SaveChangesAsync();
        Log.Error("Order with this GUID {} confirmed successfully", id);
        return order;
    }
    
    private string GetMyEmail()
    {
        var email = GetMyClaimValue(ClaimTypes.Name);
        EmailUtil.CheckEmailExists(email);
        return email;
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
    
    private async Task<Basket?> GetBasket(string userEmail)
    {
        var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserEmail == userEmail);
        CheckBasketNotFound(basket);
        CheckBasketEmpty(basket);
        return basket;
    }
    
    private void CheckBasketNotFound(Basket? basket)
    {
        if (basket == null)
        {
            throw new BasketNotFoundException("Basket not found");
        }
    }

    private void CheckBasketEmpty(Basket basket)
    {
        if (basket.Dishes.Count == 0)
        {
            throw new BasketEmptyException("Basket is empty");
        }
    }

    private Order InitNewOrder(OrderCreateRequest order, string userEmail, Basket? basket)
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
        return newOrder;
    }
    
    private async Task<Order> GetDeliveredOrder(Guid id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        CheckOrderNotFound(order);
        CheckOrderIsDelivered(order);
        CheckBelongingOrder(order);
        return order;
    }
    
    private void CheckOrderNotFound(Order? order)
    {
        if (order == null)
        {
            throw new OrderNotFoundException("Order not found");
        }
    }
    
    private void CheckOrderIsDelivered(Order? order)
    {
        if (order != null && order.Status == OrderStatus.Delivered)
        {
            throw new OrderAlreadyConfirmedException("Order already delivered");
        }
    }

    private void CheckBelongingOrder(Order order)
    {
        if (order.UserEmail != GetMyEmail())
        {
            throw new OrderNotFoundException("Order not found");
        }
    }
}