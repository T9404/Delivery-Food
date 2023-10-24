using WebApplication.Data;
using WebApplication.Entity;
using WebApplication.Enums;
using WebApplication.Models;

namespace WebApplication.Services.Impl;

public class AddressServiceImpl : AddressService
{
    private readonly DataBaseContext _dataBaseContext;

    public AddressServiceImpl(DataBaseContext dataBaseContext)
    {
        _dataBaseContext = dataBaseContext;
    }

    public Task<List<SearchAddressResponse>> Search(int parentObjectId, string query)
    {
        var result = new List<SearchAddressResponse>();
        var relationHierarchy = _dataBaseContext.HierarchyAddresses
            .Where(hierarchyAddress => hierarchyAddress.ParentObjectId == parentObjectId)
            .ToList();

        foreach (var hierarchyAddress in relationHierarchy)
        {
            result.Add(new SearchAddressResponse(hierarchyAddress.ObjectId, "", "", ObjectLevelAddresses.Region, ""));
        }

        result = fillToDto(result);
        if (query != null)
        {
            result = result.Where(address => address.text.Contains(query)).ToList();
        }
        return Task.FromResult(result);
    }

    private List<SearchAddressResponse> fillToDto(List<SearchAddressResponse> result)
    {
        for (var i = 0; i < result.Count; i++)
        {
            AddressBeforeHouse address = _dataBaseContext.AddressBeforeHouses
                .FirstOrDefault(addressBeforeHouse => addressBeforeHouse.ObjectId == result[i].objectId);
            if (address != null)
            {
                result[i].objectGuid = address.ObjectGuid;
                result[i].text = address.TypeName + " " + address.Name;
                result[i].objectLevel = converterToEnumAddresses(address.Level);
                result[i].objectLevelText = convertLevelToString(address.Level);
            }
        }
        
        for (var i = 0; i < result.Count; i++)
        {
            AddressAfterHouse address = _dataBaseContext.AddressAfterHouse
                .FirstOrDefault(addressHouse => addressHouse.ObjectId == result[i].objectId);
            if (address != null)
            {
                result[i].objectGuid = address.ObjectGuid;
                result[i].text = address.HouseNum;
                result[i].objectLevel =  ObjectLevelAddresses.Building;
                result[i].objectLevelText = convertHouseTypeToString(address.HouseType);
            }
        }
        
        // delete empty objects
        for (var i = 0; i < result.Count; i++)
        {
            if (result[i].objectGuid == "")
            {
                result.RemoveAt(i);
                i--;
            }
        }
        return result;
    }

    private ObjectLevelAddresses converterToEnumAddresses(int? level)
    {
        switch (level)
        {
            case 1:
                return ObjectLevelAddresses.Region;
            case 2:
                return ObjectLevelAddresses.AdministrativeArea;
            case 3:
                return ObjectLevelAddresses.MunicipalArea;
            case 4:
                return ObjectLevelAddresses.RuralUrbanSettlement;
            case 5:
                return ObjectLevelAddresses.City;
            case 6:
                return ObjectLevelAddresses.Locality;
            case 7:
                return ObjectLevelAddresses.ElementOfPlanningStructure;
            case 8:
                return ObjectLevelAddresses.ElementOfRoadNetwork;
            case 9:
                return ObjectLevelAddresses.Land;
            case 10:
                return ObjectLevelAddresses.Building;
            case 11:
                return ObjectLevelAddresses.Room;
            case 12:
                return ObjectLevelAddresses.RoomInRooms;
            case 13:
                return ObjectLevelAddresses.AutonomousRegionLevel;
            case 14:
                return ObjectLevelAddresses.IntracityLevel;
            case 15:
                return ObjectLevelAddresses.AdditionalTerritoriesLevel;
            case 16:
                return ObjectLevelAddresses.LevelOfObjectsInAdditionalTerritories;
            case 17:
                return ObjectLevelAddresses.CarPlace;
            default:
                return ObjectLevelAddresses.Region;
        }
    }

    private string convertLevelToString(int? level)
    {
        switch (level)
        {
            case 1:
                return "Субъект РФ";
            case 2:
                return "Административный район";
            case 3:
                return "Муниципальный район";
            case 4:
                return "Сельское/городское поселение";
            case 5:
                return "Город";
            case 6:
                return "Населенный пункт";
            case 7:
                return "Элемент планировочной структуры";
            case 8:
                return "Элемент улично-дорожной сети";
            case 9:
                return "Земельный участок";
            case 10:
                return "Здание (сооружение)";
            case 11:
                return "Помещение";
            case 12:
                return "Помещения в пределах помещения";
            case 13:
                return "Уровень автономного округа (устаревшее)";
            case 14:
                return "Уровень внутригородской территории (устаревшее)";
            case 15:
                return "Уровень дополнительных территорий (устаревшее)";
            case 16:
                return "Уровень объектов на дополнительных территориях (устаревшее)";
            case 17:
                return "Машино-место";
            default:
                return "";
        }
    }

    private string convertHouseTypeToString(int? houseType)
    {
        switch (houseType)
        {
            case 1:
                return "Владение";
            case 2:
                return "Дом";
            case 3:
                return "Домовладение";
            case 4:
                return "Гараж";
            case 5:
                return "Здание";
            case 6:
                return "Шахта";
            case 7:
                return "Строение";
            case 8:
                return "Сооружение";
            case 9:
                return "Литера";
            case 10:
                return "Корпус";
            case 11:
                return "Подвал";
            case 12:
                return "Котельная";
            case 13:
                return "Погреб";
            case 14:
                return "Объект незавершенного строительства";
            default:
                return "";
        }
    }
    

    public Task<List<SearchAddressResponse>> GetChain(string objectGuid)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<SearchAddressResponse>> GetAddressChain(string objectGuid)
    {
        throw new NotImplementedException();
    }
}