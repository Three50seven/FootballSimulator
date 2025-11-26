namespace Common.Core.Domain
{
    public static class HistoricalEntityExtensions
    {
        /// <summary>
        /// Creates EntityHistory object and calls <see cref="IEntityHistoryUpdated.OnHistoryUpdate(EntityHistory)"/> on the provided entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="lastUpdated"></param>
        /// <returns></returns>
        public static EntityHistory SetAddedHistoryValue<T>(this T entity, int entityTypeId, CommandDate lastUpdated)
             where T : class, IHistoricalEntity
        {
            var addedHistoryRecord = new EntityHistory(
                                            entityTypeId,
                                            CommandTypeOption.Added,
                                            entity.Guid,
                                            new UserCommandEvent(lastUpdated.UserId, lastUpdated.Date));

            entity.OnHistoryUpdate(addedHistoryRecord);

            return addedHistoryRecord;
        }
    }
}
