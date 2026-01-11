using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class Season : DomainEntity
    {
        private Season() { }
        public Season(int year, int typeId, string? name, DateTime beginDate, DateTime endDate)
        {
            Year = year;
            TypeId = typeId;
            Name = name;
            BeginDate = beginDate;
            EndDate = endDate;
        }
        public int Year { get; set; }
        public int TypeId { get; set; }
        public SeasonType? Type { get; set; }
        public string? Name { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<SeasonWeek> Weeks { get; set; } = [];
    }
}
