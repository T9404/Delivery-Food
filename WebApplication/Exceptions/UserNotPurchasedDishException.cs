namespace WebApplication.Exceptions;

public class UserNotPurchasedDishException : Exception
{
    public UserNotPurchasedDishException(string message) : base(message)
    {
    }
}