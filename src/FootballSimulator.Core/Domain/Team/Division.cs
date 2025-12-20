using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class Division : LookupEntity
    {
        private Division() { }
        public Division(string name) : base(name) { }
        public int ConferenceId { get; set; }
        public Conference? Conference { get; set; } = null;
        public IEnumerable<Team> Teams { get; set; } = new List<Team>();
    }
}
