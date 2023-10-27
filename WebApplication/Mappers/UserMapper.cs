using WebApplication.Entities;
using WebApplication.Models.Responses;

namespace WebApplication.Mappers;

internal static class UserMapper
{
    public static UserProfileResponse EntityToUserDto(User user)
    {
        UserProfileResponse userProfileResponse = new()
        {
            FullName = user.FullName,
            BirthDate = user.BirthDate,
            Gender = user.Gender.ToString(),
            Address = user.Address,
            Email = user.Email,
            Phone = user.Phone
        };
        return userProfileResponse;
    }
}