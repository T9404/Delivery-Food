using WebApplication.Models.Responses;

namespace WebApplication.Services;

public interface AddressService
{
    Task<List<SearchAddressResponse>> Search(int parentObjectId, string query);
    Task<List<SearchAddressResponse>> GetChain(string objectGuid);
    Task<List<SearchAddressResponse>> GetAddressChain(string objectGuid);
}