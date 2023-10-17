namespace WebApplication.Models.Responses;

public record RegistrationResponse(string FullName, string Email)
{
    public RegistrationResponse() : this(string.Empty, string.Empty)
    {
    }
}