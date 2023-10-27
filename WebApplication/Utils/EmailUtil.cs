using WebApplication.Exceptions;

namespace WebApplication.Utils;

internal static class EmailUtil
{
    public static void CheckEmailExists(string? email)
    {
        if (email == null)
        {
            throw new UserNotFoundException("User with this email not found");
        }
    }
}