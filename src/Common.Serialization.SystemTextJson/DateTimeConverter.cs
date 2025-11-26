using Common.Core;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Serialization.SystemTextJson
{
    /// <summary>
    /// Custom JsonConverter for DateTime values to parse date on read 
    /// and to write date out in a full format via <see cref="DateTimeFormats.FullDateTime"/> for proper UTC output.
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Format(DateTimeFormats.FullDateTime));
        }
    }
}
