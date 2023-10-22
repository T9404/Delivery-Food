using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Entity;

[Table("baskets")]
public class Basket
{
    [Key]
    [Column("user_email")]
    public string UserEmail { get; set; } = null!;
    
    [Column("total_price")]
    public int TotalPrice { get; set; }
    
    [Column("dishes")]
    public List<Dish> Dishes { get; set; } = null!;
}
