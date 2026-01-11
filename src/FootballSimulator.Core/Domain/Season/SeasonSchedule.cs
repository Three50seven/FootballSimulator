using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class SeasonSchedule : DomainEntity
    {
        private SeasonSchedule() { }
        public SeasonSchedule(int weekId)
        {
            WeekId = weekId;
        }
        public int WeekId { get; set; }
        public SeasonWeek? SeasonWeek { get; set; }
        public int HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }
        DateTime? DateScheduled { get; set; }
        public IEnumerable<Game>? Games { get; set; } = [];
    }
}
