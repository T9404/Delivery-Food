namespace WebApplication.Exceptions;

public class AddressPathNotFoundException : Exception
{
    public AddressPathNotFoundException(string message) : base(message)
    {
    }
}