using System.Linq;

namespace Common.Core.Domain
{
    public static class SlugQueryExtensions
    {
        /// <summary>
        /// Query by <see cref="ISlug.Slug"/>. Filter is lowered and also checked without hyphens.
        /// </summary>
        /// <typeparam name="TEntitySlug"></typeparam>
        /// <param name="query"></param>
        /// <param name="slug"></param>
        /// <returns></returns>
        public static IQueryable<TEntitySlug> BySlug<TEntitySlug>(this IQueryable<TEntitySlug> query, string slug)
            where TEntitySlug : class, ISlug
        {
            if (query == null || string.IsNullOrWhiteSpace(slug))
                return query;

            slug = slug.SetNullToEmpty().ToLower();

            return query.Where(e => e.Slug == slug || e.Slug.Replace("-", "") == slug);
        }
    }
}
