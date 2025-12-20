using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class Conference : LookupEntity
    {
        private Conference() { }
        public Conference(string name) : base(name) { }
        public string? Abbreviation { get; set; }
        public IEnumerable<Division> Divisions { get; set; } = new List<Division>();
    }
}
