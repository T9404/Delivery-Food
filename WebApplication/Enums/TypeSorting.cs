using System.Text.Json.Serialization;

namespace WebApplication.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TypeSorting
{
    NameAsc,
    NameDesc,
    PriceAsc,
    PriceDesc,
    RatingAsc,
    RatingDesc
}