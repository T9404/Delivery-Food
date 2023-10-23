using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApplication.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishCategory
{
    All,
    Wok,
    Pizza,
    Soup,
    Desert,
    Drink
}