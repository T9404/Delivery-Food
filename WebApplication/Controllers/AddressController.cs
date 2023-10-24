using Microsoft.AspNetCore.Mvc;
using WebApplication.Models.Responses;

namespace WebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly AddressService _addressService;

    public AddressController(AddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("/search")]
    public async Task<List<SearchAddressResponse>> Search([FromQuery] )
    {
        
    }
}