using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class SeasonByeWeek : DomainEntity
    {
        private SeasonByeWeek() { }
        public SeasonByeWeek(int teamId, int seasonWeekId)
        {
            TeamId = teamId;
            SeasonWeekId = seasonWeekId;
        }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        public int SeasonWeekId { get; set; }
        public SeasonWeek? SeasonWeek { get; set; }
    }
}
