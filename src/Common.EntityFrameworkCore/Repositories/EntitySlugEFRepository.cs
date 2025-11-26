using Common.Core;
using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFrameworkCore
{
    public class EntitySlugEFRepository : IEntitySlugRepository
    {
        public EntitySlugEFRepository(DbContext context)
        {
            Context = context;
        }

        protected DbContext Context { get; }

        public virtual IEnumerable<string> GetAnyMatchingUrls<TType, TKey>(string url, TKey entityId)
            where TType : class, ISlug, IEntity<TKey>
        {
            return BuildQuery<TType, TKey>(url, entityId).ToList();
        }

        public virtual async Task<IEnumerable<string>> GetAnyMatchingUrlsAsync<TType, TKey>(string url, TKey entityId)
            where TType : class, ISlug, IEntity<TKey>
        {
            return await BuildQuery<TType, TKey>(url, entityId).ToListAsync();
        }

        protected virtual IQueryable<string> BuildQuery<TType, TKey>(string url, TKey entityId)
            where TType : class, ISlug, IEntity<TKey>
        {
            var query = Context.Set<TType>()
                               .AsNoTracking()
                               .Where(x => EF.Functions.Like(x.Slug, $"{url}%"));

            if (typeof(IArchivable).IsAssignableFrom(typeof(TType)))
                query = query.Where(x => !(x as IArchivable).Archive);

            if (entityId != null && !entityId.Equals(default(TKey)))
                query = query.Where(DbSetExtensions.IdNotEqualsPredicate<TType, TKey>(entityId));

            return query.Select(x => x.Slug);
        }
    }
}