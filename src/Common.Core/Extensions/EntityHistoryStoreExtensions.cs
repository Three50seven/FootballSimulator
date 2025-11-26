using Common.Core.Domain;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class EntityHistoryStoreExtensions
    {
        /// <summary>
        /// Process a simple command event that will only update an entities create/update values.
        /// Does not insert new history records and does not require a designated EntityTypeId.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entityHistoryStore"></param>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Task ProcessNoTrackCommandAsync<TEntity, TKey>(
            this IEntityHistoryStore entityHistoryStore, TEntity entity, int userId)
            where TEntity : class, IEntity<TKey>, IHistoricalEntity, IArchivable
        {
            var commandType = entity.Archive ? CommandTypeOption.Deleted
                : (entity.IsNew ? CommandTypeOption.Added : CommandTypeOption.Updated);

            return ProcessNoTrackCommandAsync(entityHistoryStore, entity, userId, commandType);
        }

        /// <summary>
        /// Process a simple command event that will only update an entities create/update values.
        /// Does not insert new history records and does not require a designated EntityTypeId.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityHistoryStore"></param>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static Task ProcessNoTrackCommandAsync<TEntity>(
            this IEntityHistoryStore entityHistoryStore, TEntity entity, int userId, CommandTypeOption command)
            where TEntity : class, IHistoricalEntity
        {
            return entityHistoryStore.ProcessCommandAsync(new HistoryCommandContext(
                                                            default, // entity type id does not matter here
                                                            entity,
                                                            command,
                                                            userId: userId,
                                                            storeEventRecords: false));
        }
    }
}
