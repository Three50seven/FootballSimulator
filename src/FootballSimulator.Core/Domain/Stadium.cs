using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class Stadium : FSDataEntity, IArchivable
    {
        public Stadium(string name, int capacity, int cityId, int stadiumTypeId, int climateTypeId)
        {
            Name = name;
            Capacity = capacity;
            CityId = cityId;
            StadiumTypeId = stadiumTypeId;
            ClimateTypeId = climateTypeId;
        }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public int CityId { get; private set; }
        public City? City { get; private set; }
        public int StadiumTypeId { get; private set; }
        public StadiumType? StadiumType { get; private set; }
        public int ClimateTypeId { get; private set; }
        public ClimateType? ClimateType { get; private set; }
        public bool IsSuperBowlCandidate { get; private set; } = false;
        public bool IsInternationalMatchCandidate { get; private set; } = false;
        public DateTime BrokeGround { get; private set; }
        public DateTime? Opened { get; private set; }
        public bool Archive { get; set; }
    }
}
