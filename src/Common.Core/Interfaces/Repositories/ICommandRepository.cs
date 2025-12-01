namespace Common.Core
{
    /// <summary>
    /// Generic repository for standard command operations.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICommandRepository<TEntity> : ICommandRepository<TEntity, int>
        where TEntity : class
    {

    }

    /// <summary>
    /// Generic repository for standard command operations.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface ICommandRepository<TEntity, TKey> : ICommandRepository<TEntity, TKey, int>
        where TEntity : class
    {

    }

    /// <summary>
    /// Generic repository for standard command operations.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    public interface ICommandRepository<TEntity, TKey, TUserKey>
        where TEntity : class
    {
        void AddOrUpdate(TEntity item, TUserKey userId = default!, bool? recordChangeEvent = null);
        Task AddOrUpdateAsync(TEntity item, TUserKey userId = default!, bool? recordChangeEvent = null);
        void Delete(TEntity item, TUserKey userId = default!, bool? recordChangeEvent = null);
        Task DeleteAsync(TEntity item, TUserKey userId = default!, bool? recordChangeEvent = null);

#if FEATURE_DEFAULT_INTERFACE_METHODS
        void AddOrUpdateMany(IEnumerable<TEntity> items, TUserKey userId = default!, bool? recordChangeEvent = null)
        {
            foreach (var item in items)
            {
                AddOrUpdate(item, userId, recordChangeEvent);
            }
        }

        async Task AddOrUpdateManyAsync(IEnumerable<TEntity> items, TUserKey userId = default!, bool? recordChangeEvent = null)
        {
            foreach (var item in items)
            {
                await AddOrUpdateAsync(item, userId, recordChangeEvent);
            }
        }

        void DeleteMany(IEnumerable<TEntity> items, TUserKey userId = default!, bool? recordChangeEvent = null)
        {
            foreach (var item in items)
            {
                Delete(item, userId, recordChangeEvent);
            }
        }

        async Task DeleteManyAsync(IEnumerable<TEntity> items, TUserKey userId = default!, bool? recordChangeEvent = null)
        {
            foreach (var item in items)
            {
                await DeleteAsync(item, userId, recordChangeEvent);
            }
        }
#else
        void AddOrUpdateMany(IEnumerable<TEntity> items, TUserKey userId = default, bool? recordChangeEvent = null);
        Task AddOrUpdateManyAsync(IEnumerable<TEntity> items, TUserKey userId = default, bool? recordChangeEvent = null);
        void DeleteMany(IEnumerable<TEntity> items, TUserKey userId = default, bool? recordChangeEvent = null);
        Task DeleteManyAsync(IEnumerable<TEntity> items, TUserKey userId = default, bool? recordChangeEvent = null);
#endif
    }
}
