using WebApplication.Enums;

namespace WebApplication.Models.Requests;

public record UserEdit(string FullName, DateTime BirthDate, Gender Gender, Guid AddressId, String Phone)
{
    public UserEdit(): this(String.Empty, DateTime.Today, Gender.Male, new Guid(), String.Empty)
    {
    }
}