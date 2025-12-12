using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class Country : LookupEntity
    {
        private Country() { }
        public Country(string name, string code)
        {
            Name = name;
            Code = code;
        }
        public string? Code { get; private set; }
        public IEnumerable<State> States { get; private set; } = new List<State>();
    }
}
