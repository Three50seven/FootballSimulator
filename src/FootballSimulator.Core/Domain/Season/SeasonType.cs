using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class SeasonType : LookupEntity
    {
        private SeasonType() { }
        public SeasonType(SeasonTypeOption name) : base(name) { }
    }
}