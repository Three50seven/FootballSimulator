using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class TeamRating : DomainEntity
    {
        private TeamRating() { }
        public TeamRating(int teamId, int seasonId)
        {
            TeamId = teamId;
            SeasonId = seasonId;
        }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        public int SeasonId { get; set; }
        public Season? Season { get; set; }
        public decimal OffenseRating { get; set; }
        public decimal DefenseRating { get; set; }
        public decimal SpecialTeamsRating { get; set; }
        public decimal CoachingRating { get; set; }
        public decimal OverallRating { get; set; }
        public decimal HomeFieldAdvantageRating { get; set; }
    }
}
