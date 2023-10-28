using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApplication.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
[Serializable]
public enum DishCategory
{
    All,
    Wok,
    Pizza,
    Soup,
    Dessert,
    Drink
}