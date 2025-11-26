using Common.Core;
using System.Text.Json;

namespace Common.Serialization.SystemTextJson
{
    /// <summary>
    /// Serialize functions around Microsoft's System.Text.Json <see cref="JsonSerializer"/>.
    /// </summary>
    public class Serializer : ISerializer
    {
        public static readonly JsonSerializerOptions DefaultSettings = new();

        static Serializer()
        {
            DefaultSettings.PropertyNameCaseInsensitive = true;
            DefaultSettings.PropertyNamingPolicy = null;
            DefaultSettings.Converters.Add(new DateTimeConverter());
        }

        private readonly JsonSerializerOptions _options;

        public Serializer(JsonSerializerOptions options)
        {
            _options = options ?? DefaultSettings;
        }

        public object Deserialize(string serializedValue, Type type)
        {
            if (string.IsNullOrWhiteSpace(serializedValue))
                return null!;

            return JsonSerializer.Deserialize(serializedValue, type, _options)!;
        }

        public string Serialize(object value)
        {
            if (value == null)
                return null;

            return JsonSerializer.Serialize(value, value.GetType(), _options);
        }
    }
}
