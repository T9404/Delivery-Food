namespace WebApplication.Models.Requests;

public record RefreshRequest(string RefreshToken)
{
    public RefreshRequest() : this(string.Empty) 
    {
    }
}