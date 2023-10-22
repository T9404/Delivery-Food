namespace WebApplication.Exceptions;

public class OrderAlreadyConfirmedException : Exception
{
    public OrderAlreadyConfirmedException(string message) : base(message)
    {
    }
}