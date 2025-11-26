using Common.Core;
using Newtonsoft.Json;

namespace Common.Serialization.JsonNET
{
    /// <summary>
    /// Serializer that utilizes Json.NET from Newtonsoft.
    /// </summary>
    public class Serializer : ISerializer
    {
        static Serializer()
        {
            JsonConvert.DefaultSettings = () =>
            {
                return DefaultSettings;
            };
        }

        public static readonly JsonSerializerSettings DefaultSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public Serializer(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? DefaultSettings;
        }

        public virtual object Deserialize(string serializedValue, Type type)
        {
            if (string.IsNullOrWhiteSpace(serializedValue))
                return null!;

            return JsonConvert.DeserializeObject(serializedValue, type, _jsonSerializerSettings)!;
        }

        public virtual string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.None, _jsonSerializerSettings);
        }
    }
}
