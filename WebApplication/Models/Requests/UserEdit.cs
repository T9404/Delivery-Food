using WebApplication.Enums;

namespace WebApplication.Models.Requests;

public record UserEdit(string FullName, DateTime BirthDate, Gender Gender, Guid AddressId, String Phone)
{
    public UserEdit(): this(String.Empty, DateTime.Now, Gender.Male, Guid.Empty, String.Empty)
    {
    }
}