using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Entities;

[Table("refresh_tokens")]
public class RefreshToken
{
    [Key]
    [Column("id")]  
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("token")]
    public string Token { get; set; } = null!;
    
    [Column("expires")]
    public DateTime Expires { get; set; }
    
    [Column("is_expired")]
    public bool IsExpired => DateTime.UtcNow > Expires;
    
    [Column("revoked")]
    public DateTime? Revoked { get; set; }
    
    [Column("is_active")]
    public bool IsActive => Revoked == null && !IsExpired;
    
    [Column("email")]
    public string Email { get; set; }
}