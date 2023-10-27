using WebApplication.Enums;

namespace WebApplication.Utils;

// This utility class provides methods for converting values from the GAR (FIAS Russia) database.
internal static class AddressUtil
{
    public static ObjectLevelAddresses ConvertToEnumAddresses(int? level)
    {
        ObjectLevelAddresses result = level switch
        {
            1 => ObjectLevelAddresses.Region,
            2 => ObjectLevelAddresses.AdministrativeArea,
            3 => ObjectLevelAddresses.MunicipalArea,
            4 => ObjectLevelAddresses.RuralUrbanSettlement,
            5 => ObjectLevelAddresses.City,
            6 => ObjectLevelAddresses.Locality,
            7 => ObjectLevelAddresses.ElementOfPlanningStructure,
            8 => ObjectLevelAddresses.ElementOfRoadNetwork,
            9 => ObjectLevelAddresses.Land,
            10 => ObjectLevelAddresses.Building,
            11 => ObjectLevelAddresses.Room,
            12 => ObjectLevelAddresses.RoomInRooms,
            13 => ObjectLevelAddresses.AutonomousRegionLevel,
            14 => ObjectLevelAddresses.IntracityLevel,
            15 => ObjectLevelAddresses.AdditionalTerritoriesLevel,
            16 => ObjectLevelAddresses.LevelOfObjectsInAdditionalTerritories,
            17 => ObjectLevelAddresses.CarPlace,
            _ => ObjectLevelAddresses.Region
        };
        return result;
    }
    
    public static string ConvertAddressBeforeHouseLevelToString(int? level)
    {
        var result = level switch
        {
            1 => "Субъект РФ",
            2 => "Административный район",
            3 => "Муниципальный район",
            4 => "Сельское/городское поселение",
            5 => "Город",
            6 => "Населенный пункт",
            7 => "Элемент планировочной структуры",
            8 => "Элемент улично-дорожной сети",
            9 => "Земельный участок",
            10 => "Здание (сооружение)",
            11 => "Помещение",
            12 => "Помещения в пределах помещения",
            13 => "Уровень автономного округа (устаревшее)",
            14 => "Уровень внутригородской территории (устаревшее)",
            15 => "Уровень дополнительных территорий (устаревшее)",
            16 => "Уровень объектов на дополнительных территориях (устаревшее)",
            17 => "Машино-место",
            _ => ""
        };
        return result;
    }
    
    public static string ConvertHouseTypeToString(int? houseType)
    {
        var result = houseType switch
        {
            1 => "Владение",
            2 => "Здание (сооружение)",
            3 => "Дом",
            4 => "Гараж",
            5 => "Здание",
            6 => "Шахта",
            7 => "Строение",
            8 => "Сооружение",
            9 => "Литера",
            10 => "Корпус",
            11 => "Подвал",
            12 => "Котельная",
            13 => "Погреб",
            14 => "Объект незавершенного строительства",
            _ => ""
        };
        return result;
    }
    
    public static string ConvertHouseNameToString(int? houseType)
    {
        var result = houseType switch
        {
            1 => "корпус",
            2 => "строение",
            3 => "сооружение",
            4 => "литера",
            _ => ""
        };
        return result;
    }
}