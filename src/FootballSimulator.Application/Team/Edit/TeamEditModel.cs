namespace FootballSimulator.Application.Models
{
    public class TeamEditModel : EditModelBase
    {
        public string? Name { get; set; }
        public string? Mascot { get; set; }
        public int FoundedYear { get; set; }
        public int StadiumId { get; set; }
        public int DivisionId { get; set; }
    }
}
