using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class TeamSeasonStatistic : DomainEntity
    {
        private TeamSeasonStatistic() { }
        public TeamSeasonStatistic(int teamId, int seasonId)
        {
            TeamId = teamId;
            SeasonId = seasonId;
        }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        public int SeasonId { get; set; }
        public Season? Season { get; set; }
        public int PostSeasonId { get; set; }
        public Season? PostSeason { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public decimal WinPercentage { get; set; }
        public int HomeWins { get; set; }
        public int HomeGames { get; set; }
        public decimal HomeWinPercentage { get; set; }
        public int ConferenceWins { get; set; }
        public int ConferenceLosses { get; set; }
        public int ConferenceTies { get; set; }
        public decimal ConferenceWinPercentage { get; set; }
        public int DivisionWins { get; set; }
        public int DivisionLosses { get; set; }
        public int DivisionTies { get; set; }
        public decimal DivisionWinPercentage { get; set; }
        public int PointsFor { get; set; }
        public int PointsAgainst { get; set; }
        public int PointDifferential { get; set; }
        public decimal StrengthOfSchedule { get; set; }
        public decimal StrengthOfVictory { get; set; }
        public decimal DivisionRank { get; set; }
        public int ConferenceRank { get; set; }
        public int? QualificationTypeId { get; set; }
        public QualificationType? QualificationType { get; set; }
        public int ConferenceSeeding { get; set; }
        public int FinalLeagueRank { get; set; }
    }
}
