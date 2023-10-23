namespace WebApplication.Models.Requests;

public record OrderCreateRequest(DateTime DeliveryTime, string AddressId)
{
    public OrderCreateRequest() : this(DateTime.Now, "") { }
}