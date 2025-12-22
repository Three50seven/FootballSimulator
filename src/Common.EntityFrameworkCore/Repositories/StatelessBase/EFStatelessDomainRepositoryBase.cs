using Common.Core;
using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Common.EntityFrameworkCore
{
    /// <summary>
    /// Entity Framework base repository specifically for <see cref="IDomainEntity"/>.
    /// Implements <see cref=" IDomainRepository{T}"/>.
    /// Includes <see cref="IArchivable.Archive"/> logic, <see cref="IEntityGuid.Guid"/> logic, and history/audit <see cref="IEntityHistoryStore"/> functionality.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TType"></typeparam>
    public abstract class EFStatelessDomainRepositoryBase<TContext, TType> : EFStatelessDomainRepositoryBase<TContext, TType, RepositoryIncludesDefaultOption>, IDomainRepository<TType>
        where TContext : DbContext
        where TType : class, IDomainEntity, IHistoricalEntity
    {
        protected EFStatelessDomainRepositoryBase(IDbContextFactory<TContext> context)
            : base(context)
        {
        }

        protected EFStatelessDomainRepositoryBase(IDbContextFactory<TContext> context, IEntityHistoryStore historyStore)
            : base(context, historyStore)
        {
        }

        protected override IQueryable<TType> BuildFullEntitySet(TContext db)
        {
            return db.Set<TType>();
        }

        protected override IQueryable<TType> BuildQueryable(TContext db, RepositoryIncludesDefaultOption includes)
        {
            return includes switch
            {
                RepositoryIncludesDefaultOption.All => BuildFullEntitySet(db),
                RepositoryIncludesDefaultOption.None => db.Set<TType>(),
                _ => throw new UnsupportedEnumException(includes),
            };
        }

    }

    public abstract class EFStatelessDomainRepositoryBase<TContext, TType, TIncludes> : EFStatelessRepositoryBase<TContext, TType, int, TIncludes>, IDomainRepository<TType, TIncludes>
    where TContext : DbContext
    where TType : class, IDomainEntity, IHistoricalEntity
    where TIncludes : struct, Enum
    {
        private static readonly StoreHistoryAttribute _storeHistoryAttribute;
        private static readonly bool _entityIsArchivable;

        static EFStatelessDomainRepositoryBase()
        {
            var entityType = typeof(TType);
            entityType.TryGetAttribute(out _storeHistoryAttribute, inherit: true);
            _entityIsArchivable = typeof(IArchivable).IsAssignableFrom(entityType);
        }

        protected EFStatelessDomainRepositoryBase(IDbContextFactory<TContext> factory)
            : this(factory, null)
        {
        }

        protected EFStatelessDomainRepositoryBase(
            IDbContextFactory<TContext> factory,
            IEntityHistoryStore historyStore)
            : base(factory)
        {
            HistoryStore = historyStore;

            if (_storeHistoryAttribute != null && HistoryStore == null)
            {
                throw new InvalidOperationException(
                    $@"The entity type {typeof(TType).FullName} is set to store history via {nameof(StoreHistoryAttribute)}
but no {nameof(IEntityHistoryStore)} was supplied to {GetType().FullName}.");
            }
        }

        protected virtual IEntityHistoryStore HistoryStore { get; }

        // ------------------------------------------------------------
        // Queryable builder (adds archivable filter)
        // ------------------------------------------------------------
        protected override IQueryable<TType> BuildQueryable(TContext db, TIncludes includes)
        {
            var query = base.BuildQueryable(db, includes);

            if (_entityIsArchivable)
                query = query.Where(x => !(x as IArchivable).Archive);

            return query;
        }

        // ------------------------------------------------------------
        // AddOrUpdate
        // ------------------------------------------------------------
        public override void AddOrUpdate(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            if (_entityIsArchivable)
                (item as IArchivable).Archive = false;

            EntityEntry<TType> entry;
            CommandTypeOption commandType;

            if (item.IsNew)
            {
                entry = db.Set<TType>().Add(item);
                commandType = CommandTypeOption.Added;
            }
            else
            {
                entry = db.Set<TType>().Update(item);
                commandType = CommandTypeOption.Updated;
            }

            if (recordChangeEvent ?? true)
                ProcessCommandHistory(db, item, entry, commandType, userId);

            db.SaveChanges();
        }

        public override async Task AddOrUpdateAsync(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            if (_entityIsArchivable)
                (item as IArchivable).Archive = false;

            EntityEntry<TType> entry;
            CommandTypeOption commandType;

            if (item.IsNew)
            {
                entry = await db.Set<TType>().AddAsync(item);
                commandType = CommandTypeOption.Added;
            }
            else
            {
                entry = db.Set<TType>().Update(item);
                commandType = CommandTypeOption.Updated;
            }

            if (recordChangeEvent ?? true)
                await ProcessCommandHistoryAsync(db, item, entry, commandType, userId);

            await db.SaveChangesAsync();
        }

        // ------------------------------------------------------------
        // Delete
        // ------------------------------------------------------------
        public override void Delete(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            EntityEntry<TType> entry;

            if (_entityIsArchivable)
            {
                (item as IArchivable).Archive = true;
                entry = db.Set<TType>().Update(item);
            }
            else
            {
                entry = db.Set<TType>().Remove(item);
            }

            if (recordChangeEvent ?? true)
                ProcessCommandHistory(db, item, entry, CommandTypeOption.Deleted, userId);

            db.SaveChanges();
        }

        public override async Task DeleteAsync(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            EntityEntry<TType> entry;

            if (_entityIsArchivable)
            {
                (item as IArchivable).Archive = true;
                entry = db.Set<TType>().Update(item);
            }
            else
            {
                entry = db.Set<TType>().Remove(item);
            }

            if (recordChangeEvent ?? true)
                await ProcessCommandHistoryAsync(db, item, entry, CommandTypeOption.Deleted, userId);

            await db.SaveChangesAsync();
        }

        // ------------------------------------------------------------
        // GetById / GetByGuid / GetAll
        // ------------------------------------------------------------
        public override TType GetById(int id, TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, includes).FirstOrDefault(x => x.Id == id);
        }

        public override async Task<TType> GetByIdAsync(int id, TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, includes).FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual TType GetByGuid(Guid guid, TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, includes).FirstOrDefault(x => x.Guid == guid);
        }

        public virtual async Task<TType> GetByGuidAsync(Guid guid, TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, includes).FirstOrDefaultAsync(x => x.Guid == guid);
        }

        public virtual IEnumerable<TType> GetAll(IEnumerable<Guid> guids, TIncludes includes = default)
        {
            if (guids == null || guids.Any())
                return null;

            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, includes).Where(x => guids.Contains(x.Guid)).ToList();
        }

        public virtual async Task<IEnumerable<TType>> GetAllAsync(IEnumerable<Guid> guids, TIncludes includes = default)
        {
            if (guids == null || guids.Any())
                return null;

            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, includes).Where(x => guids.Contains(x.Guid)).ToListAsync();
        }

        public override IEnumerable<TType> GetAll(TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, includes).ToList();
        }

        public override async Task<IEnumerable<TType>> GetAllAsync(TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, includes).ToListAsync();
        }

        public override IEnumerable<TType> GetAll(IEnumerable<int> ids, TIncludes includes = default)
        {
            if (ids == null || !ids.Any())
                return Enumerable.Empty<TType>();

            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, includes).Where(x => ids.Contains(x.Id)).ToList();
        }

        public override async Task<IEnumerable<TType>> GetAllAsync(IEnumerable<int> ids, TIncludes includes = default)
        {
            if (ids == null || !ids.Any())
                return Enumerable.Empty<TType>();

            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, includes).Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual int GetId(Guid guid)
        {
            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, default).Where(x => x.Guid == guid).Select(x => x.Id).FirstOrDefault();
        }

        public virtual async Task<int> GetIdAsync(Guid guid)
        {
            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, default).Where(x => x.Guid == guid).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public virtual Guid GetGuid(int id)
        {
            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, default).Where(x => x.Id == id).Select(x => x.Guid).FirstOrDefault();
        }

        public virtual async Task<Guid> GetGuidAsync(int id)
        {
            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, default).Where(x => x.Id == id).Select(x => x.Guid).FirstOrDefaultAsync();
        }

        // ------------------------------------------------------------
        // History
        // ------------------------------------------------------------
        protected virtual HistoryCommandContext BuildHistoryContext(
            TContext db,
            TType item,
            EntityEntry entry,
            StoreHistoryAttribute storeHistoryAttribute,
            CommandTypeOption commandType,
            int userId)
        {
            if (storeHistoryAttribute == null)
                throw new InvalidOperationException(
                    $"Attribute {typeof(StoreHistoryAttribute).FullName} is required for building history context.");

            IEnumerable<EntityPropertyChange> changes = null;

            if (storeHistoryAttribute.RecordAllEvents &&
                storeHistoryAttribute.IncludePropChanges &&
                commandType != CommandTypeOption.Deleted)
            {
                db.ChangeTracker.DetectChanges();
                changes = entry.GetAllTrackableChanges(db.ChangeTracker.Entries());
            }

            return new HistoryCommandContext(
                storeHistoryAttribute.EntityTypeId,
                item,
                commandType,
                userId,
                changes,
                storeHistoryAttribute.RecordAllEvents);
        }

        protected virtual void ProcessCommandHistory(
            TContext db,
            TType item,
            EntityEntry entry,
            CommandTypeOption commandType,
            int userId)
        {
            if (HistoryStore == null || _storeHistoryAttribute == null)
                return;

            var historyContext = BuildHistoryContext(db, item, entry, _storeHistoryAttribute, commandType, userId);
            HistoryStore.ProcessCommand(historyContext);
        }

        protected virtual Task ProcessCommandHistoryAsync(
            TContext db,
            TType item,
            EntityEntry entry,
            CommandTypeOption commandType,
            int userId)
        {
            if (HistoryStore == null || _storeHistoryAttribute == null)
                return Task.CompletedTask;

            var historyContext = BuildHistoryContext(db, item, entry, _storeHistoryAttribute, commandType, userId);
            return HistoryStore.ProcessCommandAsync(historyContext);
        }       
    }
}