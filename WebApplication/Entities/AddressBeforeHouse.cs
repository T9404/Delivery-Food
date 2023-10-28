using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Entities;

[Table("AddressBeforeHouses")]
public class AddressBeforeHouse
{
    [Key] 
    [Column("id")] 
    public int Id { get; set; }
    
    [Column("objectid")]
    public int ObjectId { get; set; }

    [Column("objectguid")] 
    public string? ObjectGuid { get; set; }
    
    [Column("changeid")]
    public int? ChangeId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("typename")]
    public string? TypeName { get; set; }
    
    [Column("level")]
    public int? Level { get; set; }
}