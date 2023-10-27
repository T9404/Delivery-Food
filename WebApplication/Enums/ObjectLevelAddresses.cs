using System.Text.Json.Serialization;

namespace WebApplication.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ObjectLevelAddresses
{
    Region, 
    AdministrativeArea, 
    MunicipalArea, 
    RuralUrbanSettlement, 
    City, 
    Locality, 
    ElementOfPlanningStructure, 
    ElementOfRoadNetwork, 
    Land,
    Building, 
    Room, 
    RoomInRooms, 
    AutonomousRegionLevel, 
    IntracityLevel, 
    AdditionalTerritoriesLevel, 
    LevelOfObjectsInAdditionalTerritories, 
    CarPlace
}