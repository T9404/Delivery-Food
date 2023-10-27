namespace WebApplication.Exceptions;

public class ExpiredTokenException : Exception
{
    public ExpiredTokenException(string message) : base(message)
    {
    }
}