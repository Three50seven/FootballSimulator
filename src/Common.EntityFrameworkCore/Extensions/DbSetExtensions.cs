using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.EntityFrameworkCore
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// Adds new entities to database or updates entities in database based on Id.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="records"></param>
        public static void AddOrUpdate<TEntity, TKey>(this DbSet<TEntity> dbSet, params TEntity[] records)
            where TEntity : class, IEntity<TKey>
        {
            foreach (var data in records)
            {
                if (dbSet.AsNoTracking().Any(IdEqualsPredicate<TEntity, TKey>(data.Id)))
                    dbSet.Update(data);
                else
                    dbSet.Add(data);
            }
        }

        /// <summary>
        /// Id comparing for generic key == key.
        /// Reference: http://stackoverflow.com/questions/40132380/ef-cannot-apply-operator-to-operands-of-type-tid-and-tid
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> IdEqualsPredicate<TEntity, TKey>(TKey id)
            where TEntity : class, IEntity<TKey>
        {
            Expression<Func<TEntity, TKey>> selector = x => x.Id;
            Expression<Func<TKey>> closure = () => id;
            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(selector.Body, closure.Body), selector.Parameters);
        }

        /// <summary>
        /// Id comparing for generic key != key.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> IdNotEqualsPredicate<TEntity, TKey>(TKey id)
            where TEntity : class, IEntity<TKey>
        {
            Expression<Func<TEntity, TKey>> selector = x => x.Id;
            Expression<Func<TKey>> closure = () => id;
            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.NotEqual(selector.Body, closure.Body), selector.Parameters);
        }

        /// <summary>
        /// Sets previous historical values (date, user) on the entity and adds entity to DbSet.
        /// Intended to be used when migrating data from old system to new system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="addedEvent"></param>
        /// <returns></returns>
        public static async Task<EntityHistory> AddNewEntityWithPrevHistoryDateAsync<T>(this DbSet<T> dbSet, T entity, int entityTypeId, CommandDate addedEvent)
            where T : class, IHistoricalEntity
        {
            var addedHistoryRecord = entity.SetAddedHistoryValue(entityTypeId, addedEvent);
            await dbSet.AddAsync(entity);
            return addedHistoryRecord;
        }

        /// <summary>
        /// Sets previous historical values (date, user) on the entity and adds entity to DbSet.
        /// Intended to be used when migrating data from old system to new system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="addedEvent"></param>
        /// <returns></returns>
        public static EntityHistory AddNewEntityWithPrevHistoryDate<T>(this DbSet<T> dbSet, T entity, int entityTypeId, CommandDate addedEvent)
            where T : class, IHistoricalEntity
        {
            var addedHistoryRecord = entity.SetAddedHistoryValue(entityTypeId, addedEvent);
            dbSet.Add(entity);
            return addedHistoryRecord;
        }
    }
}
