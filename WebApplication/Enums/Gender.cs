using System.Text.Json.Serialization;

namespace WebApplication.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male,
    Female
}