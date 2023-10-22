using Microsoft.AspNetCore.Mvc;
using WebApplication.Entity;

namespace WebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetOrders()
    {
        return Ok(await _orderService.GetOrders());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(Guid id)
    {
        return Ok(await _orderService.GetOrder(id));
    }
    
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        return Ok(await _orderService.CreateOrder(order));
    }
    
    [HttpPut("{id}/status")]
    public async Task<ActionResult<Order>> ConfirmOrder(Guid id)
    {
        return Ok(await _orderService.ConfirmOrder(id));
    }
}