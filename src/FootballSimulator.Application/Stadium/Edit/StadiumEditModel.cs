namespace FootballSimulator.Application.Models
{
    public class StadiumEditModel : EditModelBase
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public int CityId { get; set; }
        public int StadiumTypeId { get; set; }
        public int ClimateTypeId { get; set; }
    }
}
