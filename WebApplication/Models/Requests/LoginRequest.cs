namespace WebApplication.Models.Requests;

public record LoginRequest(string Email, string Password)
{
    public LoginRequest() : this(string.Empty, string.Empty)
    {
    }
}