using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class WeatherType : LookupEntity
    {
        private WeatherType() { }
        public WeatherType(string name) : base(name) { }
        public IEnumerable<ClimateType> ClimateTypes { get; set; } = new List<ClimateType>();
        public IEnumerable<ClimateTypeWeatherType> ClimateTypeWeatherTypes { get; set; } = new List<ClimateTypeWeatherType>();
    }
}
