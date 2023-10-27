using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Entities;


public class AddressHouse
{
    [Key] 
    [Column("id")] 
    public int Id { get; set; }

    [Column("objectid")] 
    public int ObjectId { get; set; }

    [Column("objectguid")] 
    public string? ObjectGuid { get; set; }
    
    [Column("changeid")]
    public string? ChangeId { get; set; }
    
    [Column("housenum")]
    public string? HouseNum { get; set; }
    
    [Column("addnum1")]
    public string? AddNum1 { get; set; }
    
    [Column("addnum2")]
    public string? AddNum2 { get; set; }
    
    [Column("housetype")]
    public int? HouseType { get; set; }
    
    [Column("addtype1")]
    public int? AddType1 { get; set; }
    
    [Column("addtype2")]
    public int? AddType2 { get; set; }
}