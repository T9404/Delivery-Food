using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApplication.Enums;

namespace WebApplication.Entity;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]  
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    public Guid Id { get; set; }
    
    [Column("name")]
    [Required]
    [StringLength(255)]
    public string FullName { get; set; } 
    
    [Column("email")]
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Column("password")]
    [Required]
    [StringLength(255, ErrorMessage = "Your password must be from 6 characters long.", MinimumLength = 6)]
    public string Password { get; set; }

    [Required] 
    [StringLength(255)] 
    public string Address { get; set; }

    [Required] 
    [RegularExpression(@"^((\+7)\s\(\d{3}\)\s\d{3}\-\d{2}\-\d{2})$", ErrorMessage = "Incorrect phone format.")]
    public string Phone { get; set; }

    [Required]
    public Gender Gender { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
}