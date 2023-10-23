using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Entity;

[Table("dishes")]
public class Dish
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = null!;
    
    [Column("description")]
    public string Description { get; set; } = null!;
    
    [Column("price")]
    public int Price { get; set; }
    
    [Column("image")]
    public string Image { get; set; } = null!;
    
    [Column("vegetarian")]
    public bool Vegetarian { get; set; }
    
    [Column("rating")]
    public double Rating { get; set; }
    
    [Column("count_ratings")]
    public int CountRatings { get; set; }
    
    [Column("category")]
    public string Category { get; set; } = null!;
}