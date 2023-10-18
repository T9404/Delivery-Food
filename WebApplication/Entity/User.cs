using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApplication.Constants;
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
    [StringLength(255, ErrorMessage = Constant.ErrorMessage.SimplePassword, MinimumLength = 6)]
    public string Password { get; set; }

    [Required] 
    [StringLength(255)] 
    public string Address { get; set; }

    [Required] 
    [RegularExpression(Constant.RegularExpression.Phone, ErrorMessage = Constant.ErrorMessage.IncorrectPhoneFormat)]
    public string Phone { get; set; }

    [Required]
    public Gender Gender { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
}