using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApplication.Enums;
using WebApplication.Mappers;

namespace WebApplication.Entities;

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
    [StringLength(255, ErrorMessage = Constants.Constants.ErrorMessage.SimplePassword, MinimumLength = 6)]
    public string Password { get; set; }

    [Column("address")]
    [Required] 
    [StringLength(255)] 
    public string Address { get; set; }

    [Column("phone")]
    [Required] 
    [RegularExpression(Constants.Constants.RegularExpression.Phone, 
        ErrorMessage = Constants.Constants.ErrorMessage.IncorrectPhoneFormat)]
    public string Phone { get; set; }

    [Column("gender")]
    [Required]
    public Gender Gender { get; set; }
    
    [Column("birth_date")]
    [Required]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime BirthDate { get; set; }
    
    [Column("role")]
    public string Role { get; set; } = Constants.Constants.Roles.User;
}