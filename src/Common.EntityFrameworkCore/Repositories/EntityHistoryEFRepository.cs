using Microsoft.EntityFrameworkCore;
using Common.Core;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class EntityHistoryEFRepository<TContextType> : EFRepositoryBase<TContextType, EntityHistory, long>, IEntityHistoryRepository
        where TContextType : DbContext, IContextHistorical
    {
        public EntityHistoryEFRepository(TContextType context)
            : base(context)
        {

        }

        protected override IQueryable<EntityHistory> EntitySet => base.EntitySet.Include(eh => eh.Type)
                                                                                .Include(eh => eh.Event.User)
                                                                                .Include(eh => eh.Changes);

        public override void AddOrUpdate(EntityHistory item, int userId = default, bool? recordChangeEvent = null)
        {
            if (!item.IsNew)
                throw new InvalidOperationException("EntityHistory must always be a new record when saving.");

            Context.AddToHistory(item);
        }

        public override Task AddOrUpdateAsync(EntityHistory item, int userId = default, bool? recordChangeEvent = null)
        {
            AddOrUpdate(item);
            return Task.CompletedTask;
        }

        public override void Delete(EntityHistory item, int userId = default, bool? recordChangeEvent = null)
        {
            throw new InvalidOperationException("EntityHistory items cannot be deleted.");
        }

        public override Task DeleteAsync(EntityHistory item, int userId = default, bool? recordChangeEvent = null)
        {
            throw new InvalidOperationException("EntityHistory items cannot be deleted.");
        }

        public virtual IEnumerable<EntityHistory> GetAllByEntity(Guid entityGuid, int typeId)
        {
            return EntitySet.AsNoTracking()
                            .Where(eh => eh.EntityGuid == entityGuid && eh.TypeId == typeId)
                            .OrderBy(eh => eh.Event.Date)
                            .ToList();
        }

        public virtual async Task<IEnumerable<EntityHistory>> GetAllByEntityAsync(Guid entityGuid, int typeId)
        {
            return await EntitySet.AsNoTracking()
                                  .Where(eh => eh.EntityGuid == entityGuid && eh.TypeId == typeId)
                                  .OrderBy(eh => eh.Event.Date)
                                  .ToListAsync();
        }

        public virtual ChangeEvents GetChangeEvents(Guid entityGuid, int typeId)
        {
            return DbSet.Include(eh => eh.Event.User)
                        .AsNoTracking()
                        .Where(eh => eh.EntityGuid == entityGuid && eh.TypeId == typeId)
                        .SelectCreatedUpdatedEvents();
        }

        public virtual Task<ChangeEvents> GetChangeEventsAsync(Guid entityGuid, int typeId)
        {
            return DbSet.Include(eh => eh.Event.User)
                        .AsNoTracking()
                        .Where(eh => eh.EntityGuid == entityGuid && eh.TypeId == typeId)
                        .SelectCreatedUpdatedEventsAsync();
        }
    }
}
