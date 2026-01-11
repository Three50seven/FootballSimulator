using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class RankingScopeType : LookupEntity
    {
        private RankingScopeType() { }
        public RankingScopeType(RankingScopeTypeOption name) : base(name)
        {
        }
    }
}
