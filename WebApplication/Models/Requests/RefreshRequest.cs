namespace WebApplication.Models.Requests;

public record RefreshRequest(string AccessToken)
{
    public RefreshRequest() : this(string.Empty) {
    }
}