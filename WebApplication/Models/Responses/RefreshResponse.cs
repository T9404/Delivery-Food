namespace WebApplication.Models.Responses;

public record RefreshResponse(string AccessToken, string RefreshToken)
{
    public RefreshResponse() : this(string.Empty, string.Empty)
    {
    }
}