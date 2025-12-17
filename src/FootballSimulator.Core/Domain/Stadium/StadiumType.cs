using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class StadiumType : LookupEntity
    {
        private StadiumType() { }
        public StadiumType(string name) : base(name) { }
    }
}
