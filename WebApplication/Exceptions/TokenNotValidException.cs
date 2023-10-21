namespace WebApplication.Exceptions;

public class TokenNotValidException : Exception
{
    public TokenNotValidException(string message) : base(message)
    {
    }
}