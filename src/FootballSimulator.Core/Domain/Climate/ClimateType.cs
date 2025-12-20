using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class ClimateType : LookupEntity
    {
        private ClimateType() { }
        public ClimateType(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public string? Description { get; private set; }
        public IEnumerable<WeatherType> WeatherTypes { get; set; } = new List<WeatherType>();
        public IEnumerable<ClimateTypeWeatherType> ClimateTypeWeatherTypes { get; set; } = new List<ClimateTypeWeatherType>();
    }
}
