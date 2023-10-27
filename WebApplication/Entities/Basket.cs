using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Entities;

[Table("baskets")]
public class Basket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("total_price")]
    public int TotalPrice { get; set; }

    [Column("dishes")] 
    public List<Guid> Dishes { get; set; } 
    
    [Column("user_email")]
    public string UserEmail { get; set; } = null!;
}
