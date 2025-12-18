namespace FootballSimulator.Core.DTOs
{
    public class StadiumSearchListItem
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? Name { get; set; }
        public int Capacity { get; set; }
        public string? CityName { get; set; }
        public string? StadiumTypeName { get; set; }
        public string? ClimateTypeName { get; set; }
    }
}
