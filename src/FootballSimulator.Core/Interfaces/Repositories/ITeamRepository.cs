using Common.Core;
using Common.Core.Domain;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.DTOs;

namespace FootballSimulator.Core.Interfaces
{
    public interface ITeamRepository : IDomainRepository<Team, TeamQueryIncludeOption>
    {
        Task<IPagedEnumerable<Team>> SearchAsync(TeamSearchFilter filter, ResultListFilter resultFilter);
    }
}
