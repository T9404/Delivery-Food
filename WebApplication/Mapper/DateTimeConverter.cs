using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication.Mapper;

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateTimeString = reader.GetString();
        var parsedDateTime = DateTime.Parse(dateTimeString);
        
        if (parsedDateTime > DateTime.Now)
        {
            throw new JsonException("Date can't be in the future");
        }

        return parsedDateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToLocalTime().ToString(Constants.Constants.Format.DateTime));
    }
}