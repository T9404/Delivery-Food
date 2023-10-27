using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Entity;


public class HierarchyAddress
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("objectid")] 
    public int ObjectId { get; set; }

    [Column("parentobjid")] 
    public int? ParentObjectId { get; set; }

    [Column("changeid")] 
    public int? ChangeId { get; set; }

    [Column("path")] 
    public string? Path { get; set; }
}