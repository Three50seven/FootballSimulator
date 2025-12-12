using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class State : LookupEntity
    {
        private State() { }
        public State(string name, int countryId, string abbreviation) : base(name)
        {
            CountryId = countryId;
            Abbreviation = abbreviation;
        }
        public string? Abbreviation { get; private set; }
        public int CountryId { get; private set; }
        public Country? Country { get; private set; }
        public string? Fips { get; private set; }

        public IEnumerable<City> Cities { get; private set; } = new List<City>();
    }
}
