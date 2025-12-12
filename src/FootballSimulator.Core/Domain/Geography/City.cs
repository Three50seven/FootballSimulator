using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class City : LookupEntity
    {
        private City() { }
        public City(string name, int stateId) : base(name) => StateId = stateId;

        public int StateId { get; private set; }
        public State? State { get; private set; }

        public override string ToString()
        {
            return $"{Name}, {State?.Abbreviation}";
        }

        public override SelectItem ToSelectItem() => new SelectItem(Id, ToString());
    }
}
