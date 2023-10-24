using WebApplication.Enums;

namespace WebApplication.Models;

public class SearchAddressResponse
{
    public int objectId { get; set; }
    public string objectGuid { get; set; }
    public string text { get; set; }
    public ObjectLevelAddresses objectLevel { get; set; }
    public string objectLevelText { get; set; }
    
    public SearchAddressResponse(int objectId, string objectGuid, string text, ObjectLevelAddresses objectLevel,
        string objectLevelText)
    {
        this.objectId = objectId;
        this.objectGuid = objectGuid;
        this.text = text;
        this.objectLevel = objectLevel;
        this.objectLevelText = objectLevelText;
    }
    
    public SearchAddressResponse() : this(0, String.Empty, String.Empty, 
        ObjectLevelAddresses.Region, String.Empty)
    {
        
    }
}