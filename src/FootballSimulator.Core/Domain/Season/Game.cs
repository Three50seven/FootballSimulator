using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class Game : DomainEntity
    {
        private Game() { }
        public Game(int seasonScheduleId, int stadiumId, int weatherTypeId)
        {
            SeasonScheduleId = seasonScheduleId;
            StadiumId = stadiumId;
            WeatherTypeId = weatherTypeId;
        }
        public int SeasonScheduleId { get; set; }
        public SeasonSchedule? SeasonSchedule { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public int? FirstQuarterHomeScore { get; set; }
        public int? FirstQuarterAwayScore { get; set; }
        public int? SecondQuarterHomeScore { get; set; }
        public int? SecondQuarterAwayScore { get; set; }
        public int? ThirdQuarterHomeScore { get; set; }
        public int? ThirdQuarterAwayScore { get; set; }
        public int? FourthQuarterHomeScore { get; set; }
        public int? FourthQuarterAwayScore { get; set; }
        public int? OvertimeHomeScore { get; set; }
        public int? OvertimeAwayScore { get; set; }
        public int? StadiumId { get; set; }
        public Stadium? Stadium { get; set; }
        public bool StadiumRoofOpen { get; set; }
        public int WeatherTypeId { get; set; }
        public WeatherType? WeatherType { get; set; }
        public int? Temperature { get; set; }
        public int? WindSpeed { get; set; }
        public string? WindDirection { get; set; }
        public int? Humidity { get; set; }
        public string? Notes { get; set; }
    }
}
