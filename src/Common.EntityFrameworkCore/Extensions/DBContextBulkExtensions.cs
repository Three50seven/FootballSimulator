using Common.Core;
using Common.Core.Validation;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFrameworkCore
{
    public static class DbContextBulkExtensions
    {
        public const int DefaultBatchSize = 100;

        /// <summary>
        /// Bulk insert large dataset of <typeparamref name="TEntity"/>.
        /// Will dispose and recreate DbContext to clear out change tracker.
        /// Requires <paramref name="onCreateDbContext"/> function to create the DbContext (typically from a factory in the calling application).
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entities">List of entities to bulk insert into the context.</param>
        /// <param name="onCreateDbContext">Required callback that creates a new context. This is used when disposing and recreating a context.</param>
        /// <param name="batchSize">Optional batch size to submit add requests. Defaults to <see cref="DefaultBatchSize"/>.</param>
        /// <returns></returns>
        public static TDbContext BulkInsert<TDbContext, TEntity>(this TDbContext context, IEnumerable<TEntity> entities,
            Func<TDbContext> onCreateDbContext, int batchSize = DefaultBatchSize)
            where TDbContext : DbContext
            where TEntity : class
        {
            if (!entities.HasItems())
                return context;

            Guard.IsNotNull(onCreateDbContext, nameof(onCreateDbContext));

            int index = 0;
            foreach (var entity in entities)
            {
                context.Set<TEntity>().Add(entity);
                index++;

                if (index % batchSize == 0)
                {
                    context.SaveChanges();
                    context.Dispose();
                    context = onCreateDbContext.Invoke();
                }
            }

            context.SaveChanges();

            return context;
        }

        /// <summary>
        /// Asynchronous bulk insert large dataset of <typeparamref name="TEntity"/>.
        /// Will dispose and recreate DbContext to clear out change tracker.
        /// Requires <paramref name="onCreateDbContext"/> function to create the DbContext (typically from a factory in the calling application).
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entities">List of entities to bulk insert into the context.</param>
        /// <param name="onCreateDbContext">Required callback that creates a new context. This is used when disposing and recreating a context.</param>
        /// <param name="batchSize">Optional batch size to submit add requests. Defaults to <see cref="DefaultBatchSize"/>.</param>
        /// <returns></returns>
        public static async Task<TDbContext> BulkInsertAsync<TDbContext, TEntity>(this TDbContext context, IEnumerable<TEntity> entities,
            Func<TDbContext> onCreateDbContext, int batchSize = DefaultBatchSize)
            where TDbContext : DbContext
            where TEntity : class
        {
            if (!entities.HasItems())
                return context;

            Guard.IsNotNull(onCreateDbContext, nameof(onCreateDbContext));

            int index = 0;
            foreach (var entity in entities)
            {
                await context.Set<TEntity>().AddAsync(entity);
                index++;

                if (index % batchSize == 0)
                {
                    await context.SaveChangesAsync();
                    context.Dispose();
                    context = onCreateDbContext.Invoke();
                }
            }

            await context.SaveChangesAsync();

            return context;
        }
    }
}
