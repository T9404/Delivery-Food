using Microsoft.AspNetCore.Mvc;
using WebApplication.Models.Responses;
using WebApplication.Services;

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
    public async Task<ActionResult<List<SearchAddressResponse>>> Search([FromQuery] int parentObjectId, string query)
    {
        return Ok(await _addressService.Search(parentObjectId, query));
    }

    [HttpGet("/chain")]
    public async Task<ActionResult<List<SearchAddressResponse>>> GetChain([FromQuery] string objectGuid)
    {
        return Ok(await _addressService.GetChain(objectGuid));
    }

    [HttpGet("/getaddresschain")]
    public async Task<ActionResult<List<SearchAddressResponse>>> GetAddressChain([FromQuery] string objectGuid)
    {
        return Ok(await _addressService.GetAddressChain(objectGuid));
    }
}