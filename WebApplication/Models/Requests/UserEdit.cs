using WebApplication.Enums;

namespace WebApplication.Models.Requests;

public record UserEdit(string FullName, DateTime BirthDate, Gender Gender, string AddressId, String Phone)
{
    public UserEdit(): this(String.Empty, DateTime.Today, Gender.Male, String.Empty, String.Empty)
    {
    }
}