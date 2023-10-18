namespace WebApplication.Models.Responses;

public record DefaultResponse(string Description, int HttpStatusCode)
{
    public DefaultResponse() : this(string.Empty, 500)
    {
    }
}