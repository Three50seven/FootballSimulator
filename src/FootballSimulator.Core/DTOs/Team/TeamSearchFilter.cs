using Common.Core;

namespace FootballSimulator.Core.DTOs
{
    public class TeamSearchFilter
    {
        public string? Name { get; set; }
        public void Clean()
        {
            Name = Name?.SetEmptyToNull()?.ToLower();
        }
    }
}
