using Common.Core;
using Common.Core.Domain;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.DTOs;

namespace FootballSimulator.Core.Interfaces
{
    public interface IStadiumRepository : IDomainRepository<Stadium, StadiumQueryIncludeOption>
    {
        Task<IPagedEnumerable<Stadium>> SearchAsync(StadiumSearchFilter filter,ResultListFilter resultFilter);
    }
}
