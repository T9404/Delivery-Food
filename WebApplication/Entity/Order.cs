﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Enums;

namespace WebApplication.Entity;

[Table("orders")]
public class Order
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("delivery_time")]
    public DateTime DeliveryTime { get; set; }
    
    [Column("order_time")]
    public DateTime OrderTime { get; set; }
    
    [Column("status")]
    public OrderStatus Status { get; set; }
    
    [Column("price")]
    public int Price { get; set; }
    
    [Column("dishes")]
    public List<Guid> Dishes { get; set; } = null!;
    
    [Column("address")]
    public string Address { get; set; } = null!;
    
    [Column("user_email")]
    public string UserEmail { get; set; } = null!;
}