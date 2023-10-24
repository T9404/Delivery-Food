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
    public async Task<List<SearchAddressResponse>> Search([FromQuery] int parentObjectId, string query)
    {
        return Ok(await _addressService.Search(parentObjectId, query));
    }

    [HttpGet("/chain")]
    public async Task<List<SearchAddressResponse>> GetChain([FromQuery] string objectGuid)
    {
        return Ok(await _addressService.GetChain(objectGuid));
    }

    [HttpGet("/getaddresschain")]
    public async Task<List<SearchAddressResponse>> GetAddressChain([FromQuery] string objectGuid)
    {
        return Ok(await _addressService.GetAddressChain(objectGuid));
    }
}