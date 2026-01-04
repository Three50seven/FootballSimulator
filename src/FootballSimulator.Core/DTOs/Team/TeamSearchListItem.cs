namespace FootballSimulator.Core.DTOs
{
    public class TeamSearchListItem
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? CityName { get; set; }
        public string? StadiumName { get; set; }
        public string? DivisionName { get; set; }
        public string? ConferenceName { get; set; }        
        public int FoundedYear { get; set; }
    }
}
