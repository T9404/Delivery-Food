namespace WebApplication.Exceptions;

public class OrderAlreadyDeliveredException : Exception
{
    public OrderAlreadyDeliveredException(string message) : base(message)
    {
    }
}