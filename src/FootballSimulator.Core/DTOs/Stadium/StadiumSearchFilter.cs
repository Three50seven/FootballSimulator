using Common.Core;

namespace FootballSimulator.Core.DTOs
{
    public class StadiumSearchFilter
    {
        public string? Name { get; set; }
        public int? MinCapacity { get; set; }
        public int? MaxCapacity { get; set; }
        public int? TypeId { get; set; }
        public int? ClimateTypeId { get; set; }
        public int? TeamId { get; set; }
        public string? CityName { get; set; }
        public string? TypeName { get; set; }

        public void Clean()
        {
            Name = Name?.SetEmptyToNull()?.ToLower();
            MinCapacity = MinCapacity.CleanForNull();
            MaxCapacity = MaxCapacity.CleanForNull();
            TypeId = TypeId.CleanForNull();
            ClimateTypeId = ClimateTypeId.CleanForNull();
            TeamId = TeamId.CleanForNull();
            CityName = CityName?.SetEmptyToNull()?.ToLower();
            TypeName = TypeName?.SetEmptyToNull()?.ToLower();
        }
    }
}
