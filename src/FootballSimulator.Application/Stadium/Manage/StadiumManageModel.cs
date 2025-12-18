using FootballSimulator.Application.Models;
using FootballSimulator.Core.DTOs;

namespace FootballSimulator.Application.Services
{
    public class StadiumManageModel : SearchModelBase<StadiumSearchListItem>
    {
        public StadiumSearchFilter Filter { get; set; } = new StadiumSearchFilter();
    }
}