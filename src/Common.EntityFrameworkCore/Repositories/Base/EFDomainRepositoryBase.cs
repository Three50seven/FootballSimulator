using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Common.EntityFrameworkCore
{
    /// <summary>
    /// Entity Framework base repository specifically for <see cref="IDomainEntity"/>.
    /// Implements <see cref=" IDomainRepository{T}"/>.
    /// Includes <see cref="IArchivable.Archive"/> logic, <see cref="IEntityGuid.Guid"/> logic, and history/audit <see cref="IEntityHistoryStore"/> functionality.
    /// </summary>
    /// <typeparam name="TContextType"></typeparam>
    /// <typeparam name="TType"></typeparam>
    public abstract class EFDomainRepositoryBase<TContextType, TType> : EFDomainRepositoryBase<TContextType, TType, RepositoryIncludesDefaultOption>, IDomainRepository<TType>
        where TContextType : DbContext
        where TType : class, IDomainEntity, IHistoricalEntity
    {
        protected EFDomainRepositoryBase(TContextType context)
            : base(context)
        {
        }

        protected EFDomainRepositoryBase(TContextType context, IEntityHistoryStore historyStore)
            : base(context, historyStore)
        {
        }

        protected override IQueryable<TType> GetEntitySet(RepositoryIncludesDefaultOption includes)
        {
            return includes switch
            {
                RepositoryIncludesDefaultOption.All => FullEntitySet,
                RepositoryIncludesDefaultOption.None => EntitySet,
                _ => throw new UnsupportedEnumException(includes),
            };
        }
    }

    /// <summary>
    /// Entity Framework base repository specifically for <see cref="IDomainEntity"/>.
    /// Implements <see cref=" IDomainRepository{T}"/>.
    /// Includes <see cref="IArchivable.Archive"/> logic, <see cref="IEntityGuid.Guid"/> logic, and history/audit <see cref="IEntityHistoryStore"/> functionality.
    /// </summary>
    /// <typeparam name="TContextType"></typeparam>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TIncludes"></typeparam>
    public abstract class EFDomainRepositoryBase<TContextType, TType, TIncludes> : EFRepositoryBase<TContextType, TType, int, TIncludes>, IDomainRepository<TType, TIncludes>
        where TContextType : DbContext
        where TType : class, IDomainEntity, IHistoricalEntity
        where TIncludes : struct, Enum
    {
        private static StoreHistoryAttribute _storeHistoryAttribute;
        private static bool _entityIsArchivable = false;

        static EFDomainRepositoryBase()
        {
            var entityType = typeof(TType);
            entityType.TryGetAttribute(out _storeHistoryAttribute, inherit: true);
            _entityIsArchivable = typeof(IArchivable).IsAssignableFrom(entityType);
        }

        protected EFDomainRepositoryBase(TContextType context)
            : this(context, null)
        {
        }

        protected EFDomainRepositoryBase(TContextType context, IEntityHistoryStore historyStore)
            : base(context)
        {
            HistoryStore = historyStore;

            if (_storeHistoryAttribute != null && HistoryStore == null)
            {
                throw new InvalidOperationException($@"The entity type {typeof(TType).FullName} is set to store history via the {nameof(StoreHistoryAttribute)} 
but an instance of {nameof(IEntityHistoryStore)} was not supplied on the repository for that entity. Pass an instance of {typeof(IEntityHistoryStore).FullName} to {typeof(EFDomainRepositoryBase<,>).FullName}
from the constructor on {GetType().FullName}.");
            }
        }

        protected virtual IEntityHistoryStore HistoryStore { get; private set; }
        protected override IQueryable<TType> EntitySet => _entityIsArchivable ? base.EntitySet.Where(x => !(x as IArchivable).Archive) : base.EntitySet;
        protected virtual IQueryable<TType> FullEntitySet => EntitySet;

        public override void AddOrUpdate(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            Guard.IsNotNull(item, nameof(item));

            if (_entityIsArchivable)
                (item as IArchivable).Archive = false;

            EntityEntry<TType> entry;
            CommandTypeOption commandType;

            if (item.IsNew)
            {
                entry = DbSet.Add(item);
                commandType = CommandTypeOption.Added;
            }
            else
            {
                entry = DbSet.Update(item);
                commandType = CommandTypeOption.Updated;
            }

            if (recordChangeEvent ?? true)
                ProcessCommandHistory(item, entry, commandType, userId);
        }

        public override async Task AddOrUpdateAsync(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            Guard.IsNotNull(item, nameof(item));

            if (_entityIsArchivable)
                (item as IArchivable).Archive = false;

            EntityEntry<TType> entry;
            CommandTypeOption commandType;

            if (item.IsNew)
            {
                entry = await DbSet.AddAsync(item);
                commandType = CommandTypeOption.Added;
            }
            else
            {
                entry = DbSet.Update(item);
                commandType = CommandTypeOption.Updated;
            }

            if (recordChangeEvent ?? true)
                await ProcessCommandHistoryAsync(item, entry, commandType, userId);
        }

        public override void Delete(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            Guard.IsNotNull(item, nameof(item));

            EntityEntry<TType> entry;
            if (_entityIsArchivable)
            {
                (item as IArchivable).Archive = true;
                entry = DbSet.Update(item);
            }
            else
            {
                entry = DbSet.Remove(item);
            }

            if (recordChangeEvent ?? true)
                ProcessCommandHistory(item, entry, CommandTypeOption.Deleted, userId);
        }

        public override async Task DeleteAsync(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            Guard.IsNotNull(item, nameof(item));

            EntityEntry<TType> entry;
            if (_entityIsArchivable)
            {
                (item as IArchivable).Archive = true;
                entry = DbSet.Update(item);
            }
            else
            {
                entry = DbSet.Remove(item);
            }

            if (recordChangeEvent ?? true)
                await ProcessCommandHistoryAsync(item, entry, CommandTypeOption.Deleted, userId);
        }

        public override TType GetById(int id, TIncludes includes = default)
        {
            return GetEntitySet(includes).FirstOrDefault(x => x.Id == id);
        }

        public override Task<TType> GetByIdAsync(int id, TIncludes includes = default)
        {
            return GetEntitySet(includes).FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual IEnumerable<TType> GetAll(IEnumerable<Guid> guids, TIncludes includes = default)
        {
            if (guids == null || guids.Any())
                return null;

            return GetEntitySet(includes).Where(x => guids.Contains(x.Guid)).ToList();
        }

        public virtual async Task<IEnumerable<TType>> GetAllAsync(IEnumerable<Guid> guids, TIncludes includes = default)
        {
            if (guids == null || guids.Any())
                return null;

            return await GetEntitySet(includes).Where(x => guids.Contains(x.Guid)).ToListAsync();
        }

        public virtual TType GetByGuid(Guid guid, TIncludes includes = default)
        {
            return GetEntitySet(includes).FirstOrDefault(x => x.Guid == guid);
        }

        public virtual Task<TType> GetByGuidAsync(Guid guid, TIncludes includes = default)
        {
            return GetEntitySet(includes).FirstOrDefaultAsync(x => x.Guid == guid);
        }

        public override IEnumerable<TType> GetAll(TIncludes includes = default)
        {
            return GetEntitySet(includes).ToList();
        }

        public override async Task<IEnumerable<TType>> GetAllAsync(TIncludes includes = default)
        {
            return await GetEntitySet(includes).ToListAsync();
        }

        public override IEnumerable<TType> GetAll(IEnumerable<int> ids, TIncludes includes = default)
        {
            if (ids == null || !ids.Any())
                return null;

            return GetEntitySet(includes).Where(x => ids.Contains(x.Id)).ToList();
        }

        public override async Task<IEnumerable<TType>> GetAllAsync(IEnumerable<int> ids, TIncludes includes = default)
        {
            if (ids == null || !ids.Any())
                return null;

            return await GetEntitySet(includes).Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual int GetId(Guid guid)
        {
            return EntitySet.Where(x => x.Guid == guid).Select(x => x.Id).FirstOrDefault();
        }

        public virtual Task<int> GetIdAsync(Guid guid)
        {
            return EntitySet.Where(x => x.Guid == guid).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public virtual Guid GetGuid(int id)
        {
            return EntitySet.Where(x => x.Id == id).Select(x => x.Guid).FirstOrDefault();
        }

        public virtual Task<Guid> GetGuidAsync(int id)
        {
            return EntitySet.Where(x => x.Id == id).Select(x => x.Guid).FirstOrDefaultAsync();
        }

        protected virtual HistoryCommandContext BuildHistoryContext(
            TType item,
            EntityEntry entry,
            StoreHistoryAttribute storeHistoryAttribute,
            CommandTypeOption commandType,
            int userId)
        {
            if (storeHistoryAttribute == null)
                throw new InvalidOperationException($"Attribute {typeof(StoreHistoryAttribute).FullName} is required on entity for building history context.");

            IEnumerable<EntityPropertyChange> changes = null;

            if (storeHistoryAttribute.RecordAllEvents && storeHistoryAttribute.IncludePropChanges && commandType != CommandTypeOption.Deleted)
            {
                Context.ChangeTracker.DetectChanges();
                changes = entry.GetAllTrackableChanges(Context.ChangeTracker.Entries());
            }

            return new HistoryCommandContext(
                storeHistoryAttribute.EntityTypeId,
                item,
                commandType,
                userId,
                changes,
                storeHistoryAttribute.RecordAllEvents);
        }

        protected virtual void ProcessCommandHistory(TType item, EntityEntry entry, CommandTypeOption commandType, int userId)
        {
            if (HistoryStore == null || _storeHistoryAttribute == null)
                return;

            var historyContext = BuildHistoryContext(item, entry, _storeHistoryAttribute, commandType, userId);
            HistoryStore.ProcessCommand(historyContext);
        }

        protected virtual Task ProcessCommandHistoryAsync(TType item, EntityEntry entry, CommandTypeOption commandType, int userId)
        {
            if (HistoryStore == null || _storeHistoryAttribute == null)
                return Task.CompletedTask;

            var historyContext = BuildHistoryContext(item, entry, _storeHistoryAttribute, commandType, userId);
            return HistoryStore.ProcessCommandAsync(historyContext);
        }
    }
}
