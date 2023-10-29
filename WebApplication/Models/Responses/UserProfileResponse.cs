namespace WebApplication.Models.Responses;

public record UserProfileResponse(string FullName, DateTime BirthDate, string Gender,
    Guid Address, string Email, string Phone)
{
    public UserProfileResponse() : this(String.Empty, DateTime.Now, String.Empty,
        new Guid(), String.Empty, String.Empty)
    {
    }
}