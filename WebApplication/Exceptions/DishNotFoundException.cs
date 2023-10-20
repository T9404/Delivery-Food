namespace WebApplication.Exceptions;

public class DishNotFoundException : Exception
{
    public DishNotFoundException(string message) : base(message)
    {
    }
}