namespace WebApplication.Models.Requests;

public record OrderCreateRequest(DateTime DeliveryTime, Guid AddressId)
{
    public OrderCreateRequest() : this(DateTime.Now, new Guid()) { }
}