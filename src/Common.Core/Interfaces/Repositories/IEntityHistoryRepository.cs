using Common.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IEntityHistoryRepository : IRepository<EntityHistory, long>
    {
        IEnumerable<EntityHistory> GetAllByEntity(Guid entityGuid, int typeId);
        Task<IEnumerable<EntityHistory>> GetAllByEntityAsync(Guid entityGuid, int typeId);
        ChangeEvents GetChangeEvents(Guid entityGuid, int typeId);
        Task<ChangeEvents> GetChangeEventsAsync(Guid entityGuid, int typeId);
    }
}
