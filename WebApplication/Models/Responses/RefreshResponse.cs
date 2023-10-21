namespace WebApplication.Models.Responses;

public record RefreshResponse(String AccessToken, String RefreshToken)
{
    public RefreshResponse() : this(string.Empty, string.Empty)
    {
    }
}