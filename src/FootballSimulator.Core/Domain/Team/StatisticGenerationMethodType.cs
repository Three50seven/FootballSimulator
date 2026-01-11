using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class StatisticGenerationMethodType : LookupEntity
    {
        private StatisticGenerationMethodType() { }
        public StatisticGenerationMethodType(StatisticGenerationMethodTypeOption name) : base(name)
        {
        }
    }
}