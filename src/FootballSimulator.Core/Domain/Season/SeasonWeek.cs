using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class SeasonWeek : DomainEntity
    {
        private SeasonWeek() { }
        public SeasonWeek(int seasonId, string? name, DateTime beginDate, DateTime endDate) 
        {
            SeasonId = seasonId;
            Name = name;
            BeginDate = beginDate;
            EndDate = endDate;
        }
        public int SeasonId { get; set; }
        public Season? Season { get; set; }
        public string? Name { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<SeasonByeWeek> SeasonByeWeeks { get; set; } = [];
        public IEnumerable<SeasonSchedule> SeasonSchedules { get; set; } = [];
    }
}
