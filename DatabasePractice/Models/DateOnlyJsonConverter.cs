using System.Text.Json.Serialization;
using System.Text.Json;

namespace DatabasePractice.Models
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
                {
                    var root = doc.RootElement;
                    int year = root.GetProperty("year").GetInt32();
                    int month = root.GetProperty("month").GetInt32();
                    int day = root.GetProperty("day").GetInt32();
                    return new DateOnly(year, month, day);
                }
            }
            return DateOnly.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}
