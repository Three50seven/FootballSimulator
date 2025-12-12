namespace FootballSimulator.Core.Domain
{
    public class ClimateTypeWeatherType
    {
        protected ClimateTypeWeatherType() { }
        public ClimateTypeWeatherType(ClimateType climateType, WeatherType weatherType)
        {
            ClimateType = climateType;
            WeatherType = weatherType;
            ClimateTypeId = climateType.Id;
            WeatherTypeId = weatherType.Id;
        }

        public ClimateTypeWeatherType(int climateTypeId, int weatherTypeId)
        {
            ClimateTypeId = climateTypeId;
            WeatherTypeId = weatherTypeId;
        }

        public int ClimateTypeId { get; private set; }
        public ClimateType? ClimateType { get; private set; }
        public int WeatherTypeId { get; private set; }
        public WeatherType? WeatherType { get; private set; }
    }
}
