using WebApplication.Models.Responses;

namespace WebApplication.Services;

public interface IAddressService
{
    Task<List<SearchAddressResponse>> Search(int parentObjectId, string? query);
    Task<List<SearchAddressResponse>> GetChain(string objectGuid);
}