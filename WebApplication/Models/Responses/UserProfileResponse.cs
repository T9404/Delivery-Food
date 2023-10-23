namespace WebApplication.Models.Responses;

public record UserProfileResponse(string FullName, DateTime BirthDate, string Gender,
    string Address, string Email, string Phone)
{
    public UserProfileResponse() : this(String.Empty, DateTime.Now, String.Empty,
        String.Empty, String.Empty, String.Empty)
    {
    }
}