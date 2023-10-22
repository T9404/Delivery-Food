namespace WebApplication.Exceptions;

public class BasketEmptyException : Exception
{
    public BasketEmptyException(string message) : base(message)
    {
    }
}