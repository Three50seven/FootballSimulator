using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class TeamPostSeasonStatistic : DomainEntity
    {
        private TeamPostSeasonStatistic() { }
        public TeamPostSeasonStatistic(int teamId, int seasonId)
        {
            TeamId = teamId;
            SeasonId = seasonId;
        }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        public int SeasonId { get; set; }
        public Season? Season { get; set; }
        public int PreviousRegularSesasonId { get; set; }
        public Season? PreviousRegularSeason { get; set; }
        public int PreviousPostSeasonId { get; set; }
        public Season? PreviousPostSeason { get; set; }
        public int PreviousFinalLeagueRank { get; set; }
        public decimal PreviousWinPercentage { get; set; }
        public int PreviousTotalPointsFor { get; set; }
        public int PreviousTotalPointsAgainst { get; set; }
        public int PreviousPointDifferential { get; set; }
        public decimal PreviousHomeWinPercentage { get; set; }
        public decimal PreviousOffenseRating { get; set; }
        public decimal PreviousDefenseRating { get; set; }
        public decimal PreviousSpecialTeamsRating { get; set; }
        public decimal PreviousCoachRating { get; set; }
        public decimal PreviousOverallRating { get; set; }
        public decimal PreviousHomeAdvantageRating { get; set; }
        public int PlayoffGamesPlayed { get; set; }
        public int PlayoffWins { get; set; }
        public int PlayoffLosses { get; set; }
        public int UpsetWins { get; set; }
        public int PlayoffPointsFor { get; set; }
        public int PlayoffPointsAgainst { get; set; }
        public int DraftPosition { get; set; }
        public decimal DraftBoostOffenseRating { get; set; }
        public decimal DraftBoostDefenseRating { get; set; }
        public decimal DraftVariance{ get; set; }
        public decimal PlayoffOffenseBonus { get; set; }
        public decimal PlayoffDefenseBonus { get; set; }
        public decimal PlayoffSpecialTeamsBonus { get; set; }
        public decimal PlayoffCoachBonus { get; set; }
        public decimal UpsetBonusTotal { get; set; }
        public decimal CalculatedOffenseRating { get; set; }
        public decimal CalculatedDefenseRating { get; set; }
        public decimal CalculatedSpecialTeamsRating { get; set; }
        public decimal CalculatedCoachRating { get; set; }
        public decimal CalculatedOverallRating { get; set; }
        public decimal HomeAdvantageRating { get; set; }
        public decimal FinalOffenseRating { get; set; }
        public decimal FinalDefenseRating { get; set; }
        public decimal FinalSpecialTeamsRating { get; set; }
        public decimal FinalCoachRating { get; set; }
        public decimal FinalOverallRating { get; set; }
        public decimal FinalHomeAdvantageRating { get; set; }
        public int StatisticGenerationMethodTypeId { get; set; }
        public StatisticGenerationMethodType? StatisticGenerationMethodType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
