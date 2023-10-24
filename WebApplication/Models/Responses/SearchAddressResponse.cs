namespace WebApplication.Models.Responses;

public record SearchAddressResponse(int objectId, string objectGuid, string text, string objectLevel,
    string objectLevelText)
{
    public SearchAddressResponse() : this(0, String.Empty, String.Empty, String.Empty, String.Empty)
    {
        
    }
}