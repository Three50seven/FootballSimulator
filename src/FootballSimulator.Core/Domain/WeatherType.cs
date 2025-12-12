using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class WeatherType : LookupEntity
    {
        private WeatherType() { }
        public WeatherType(string name) : base(name) { }
    }
}
