using WebApplication.Enums;

namespace WebApplication.Models.Responses;

public class SearchAddressResponse
{
    public int ObjectId { get; set; }
    public string ObjectGuid { get; set; }
    public string Text { get; set; }
    public ObjectLevelAddresses ObjectLevel { get; set; }
    public string ObjectLevelText { get; set; }
    
    public SearchAddressResponse(int objectId, string objectGuid, string text, ObjectLevelAddresses objectLevel,
        string objectLevelText)
    {
        ObjectId = objectId;
        ObjectGuid = objectGuid;
        Text = text;
        ObjectLevel = objectLevel;
        ObjectLevelText = objectLevelText;
    }
    
    public SearchAddressResponse() : this(0, String.Empty, String.Empty, 
        ObjectLevelAddresses.Region, String.Empty)
    {
    }
}