using FootballSimulator.Core.DTOs;

namespace FootballSimulator.Application.Models
{
    public class TeamManageModel : SearchModelBase<TeamSearchListItem>
    {
        public TeamSearchFilter Filter { get; set; } = new TeamSearchFilter();
    }
}