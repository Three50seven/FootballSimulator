using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class DivisionRotationSchedule : DomainEntity
    {
        private DivisionRotationSchedule() { }
        public DivisionRotationSchedule(int yearCycle, int gameTypeId, int divisionId, int opponentDivisionId)
        {
            YearCycle = yearCycle;
            GameTypeId = gameTypeId;
            DivisionId = divisionId;
            OponentDivisionId = opponentDivisionId;
        }
        public int YearCycle { get; set; }
        public int GameTypeId { get; set; }
        public GameType? GameType { get; set; }
        public int DivisionId { get; set; }
        public Division? Division { get; set; }
        public int OponentDivisionId { get; set; }
        public Division? OponentDivision { get; set; }
    }
}
