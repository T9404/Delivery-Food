using Serilog;
using WebApplication.Data;
using WebApplication.Entities;
using WebApplication.Enums;
using WebApplication.Exceptions;
using WebApplication.Models.Responses;
using WebApplication.Utils;

namespace WebApplication.Services.Impl;

public class AddressService : IAddressService
{
    private readonly DataBaseContext _dataBaseContext;

    public AddressService(DataBaseContext dataBaseContext)
    {
        _dataBaseContext = dataBaseContext;
    }

    public Task<List<SearchAddressResponse>> Search(int parentObjectId, string? query)
    {
        var relationHierarchy = _dataBaseContext.HierarchyAddresses
            .Where(hierarchyAddress => hierarchyAddress.ParentObjectId == parentObjectId)
            .Take(20)
            .ToList();
        
        var result = relationHierarchy.Select(hierarchyAddress => 
            new SearchAddressResponse(hierarchyAddress.ObjectId, "", "", 
                ObjectLevelAddresses.Region, "")).ToList();
        
        result = FillToDto(result);
        
        if (query != null)
        {
            result = result.Where(address => address.Text.Contains(query)).ToList();
        }
        
        Log.Information("Search address by parentObjectId: {parentObjectId}, query: {query}, result: {@result}", 
            parentObjectId, query, result);
        return Task.FromResult(result);
    }
    
    public Task<List<SearchAddressResponse>> GetChain(string objectGuid)
    {
        var objectId = GetObjectIdFromAddressBeforeHouses(objectGuid, out bool isHouse);
        var result = BuildSearchAddressResponseList(objectId, isHouse);
        Log.Information("Get chain by objectGuid: {objectGuid}, result: {@result}", objectGuid, result);
        return Task.FromResult(result);
    }

    private List<SearchAddressResponse> FillToDto(List<SearchAddressResponse> result)
    {
        for (var i = 0; i < result.Count; i++)
        {
            FillAddressResponseFromBeforeHouse(result, i);
        }
        
        for (var i = 0; i < result.Count; i++)
        {
            FillAddressResponseFromHouse(result, i);
        }
        
        RemoveElementsWithEmptyObjectGuid(result);
        return result;
    }

    private void FillAddressResponseFromBeforeHouse(List<SearchAddressResponse> result, int index)
    {
        AddressBeforeHouse? address = _dataBaseContext.AddressBeforeHouses
            .FirstOrDefault(addressBeforeHouse => addressBeforeHouse.ObjectId == result[index].ObjectId);
        if (address != null)
        {
            result[index].ObjectGuid = address.ObjectGuid ?? "";
            result[index].Text = address.TypeName + " " + address.Name;
            result[index].ObjectLevel = AddressUtil.ConvertToEnumAddresses(address.Level);
            result[index].ObjectLevelText = AddressUtil.ConvertAddressBeforeHouseLevelToString(address.Level);
        }
    }
    
    private void FillAddressResponseFromHouse(List<SearchAddressResponse> result, int index)
    {
        AddressHouse? address = _dataBaseContext.AddressAfterHouse
            .FirstOrDefault(addressHouse => addressHouse.ObjectId == result[index].ObjectId);
        if (address != null)
        {
            result[index].ObjectGuid = address.ObjectGuid ?? "";
            result[index].Text = HandleHouseName(address);
            result[index].ObjectLevel = ObjectLevelAddresses.Building;
            result[index].ObjectLevelText = AddressUtil.ConvertHouseTypeToString(address.HouseType);
        }
    }

    private void RemoveElementsWithEmptyObjectGuid(List<SearchAddressResponse> result)
    {
        for (var i = 0; i < result.Count; i++)
        {
            if (result[i].ObjectGuid == "")
            {
                result.RemoveAt(i);
                i--;
            }
        }
    }

    private int? GetObjectIdFromAddressBeforeHouses(string objectGuid, out bool isHouse)
    {
        isHouse = false;

        var objectId = _dataBaseContext.AddressBeforeHouses
            .FirstOrDefault(addressBeforeHouse => addressBeforeHouse.ObjectGuid == objectGuid)?.ObjectId;

        if (objectId == null)
        {
            isHouse = true;
            objectId = _dataBaseContext.AddressAfterHouse
                .FirstOrDefault(addressAfterHouse => addressAfterHouse.ObjectGuid == objectGuid)?.ObjectId;
        }

        if (objectId == null)
        {
            throw new AddressPathNotFoundException("Path is null");
        }

        return objectId;
    }

    private List<SearchAddressResponse> BuildSearchAddressResponseList(int? objectId, bool isHouse)
    {
        var result = new List<SearchAddressResponse>();
        var path = GetPathForObject(objectId);

        foreach (var objectIdStr in GetObjectIdsFromPath(path))
        {
            if (int.TryParse(objectIdStr, out int id))
            {
                var address = GetAddressBeforeHouse(id);
                if (address != null)
                {
                    result.Add(CreateSearchAddressResponse(address));
                }
            }
        }

        if (isHouse)
        {
            var address = GetAddressAfterHouse(objectId);
            if (address != null)
            {
                result.Add(CreateSearchAddressResponseForHouse(address));
            }
        }

        return result;
    }

    private string? GetPathForObject(int? objectId)
    {
        return _dataBaseContext.HierarchyAddresses
            .FirstOrDefault(hierarchyAddress => hierarchyAddress.ObjectId == objectId)?.Path;
    }

    private IEnumerable<string> GetObjectIdsFromPath(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return Enumerable.Empty<string>();
        }

        return path.Split(".");
    }

    private AddressBeforeHouse? GetAddressBeforeHouse(int id)
    {
        return _dataBaseContext.AddressBeforeHouses
            .FirstOrDefault(addressBeforeHouse => addressBeforeHouse.ObjectId == id);
    }

    private SearchAddressResponse CreateSearchAddressResponse(AddressBeforeHouse address)
    {
        return new SearchAddressResponse(address.ObjectId, address.ObjectGuid ?? "",
            $"{address.TypeName} {address.Name}", AddressUtil.ConvertToEnumAddresses(address.Level),
            AddressUtil.ConvertAddressBeforeHouseLevelToString(address.Level));
    }

    private AddressHouse? GetAddressAfterHouse(int? objectId)
    {
        return _dataBaseContext.AddressAfterHouse
            .FirstOrDefault(addressAfterHouse => addressAfterHouse.ObjectId == objectId);
    }

    private SearchAddressResponse CreateSearchAddressResponseForHouse(AddressHouse address)
    {
        return new SearchAddressResponse(address.ObjectId, address.ObjectGuid ?? "",
            HandleHouseName(address), ObjectLevelAddresses.Building,
            AddressUtil.ConvertHouseTypeToString(address.HouseType));
    }


    private string HandleHouseName(AddressHouse addressHouse)
    {
        var components = new List<string>();

        if (!string.IsNullOrEmpty(addressHouse.HouseNum))
        {
            components.Add(addressHouse.HouseNum);
        }

        if (addressHouse.AddType1 != null)
        {
            components.Add(AddressUtil.ConvertHouseNameToString(addressHouse.AddType1));
            components.Add(addressHouse.AddNum1 ?? "");
        }

        if (addressHouse.AddType2 != null)
        {
            components.Add(AddressUtil.ConvertHouseNameToString(addressHouse.AddType2));
            components.Add(addressHouse.AddNum2 ?? "");
        }

        return string.Join(" ", components).Trim();
    }
}