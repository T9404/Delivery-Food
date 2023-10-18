namespace WebApplication.Models.Responses;

public record DefaultResponse(string Description)
{
    public DefaultResponse() : this(string.Empty)
    {
    }
}