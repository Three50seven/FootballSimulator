using Common.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IEntitySlugRepository
    {
        IEnumerable<string> GetAnyMatchingUrls<TType, TKey>(string url, TKey entityId = default)
            where TType : class, ISlug, IEntity<TKey>;

        Task<IEnumerable<string>> GetAnyMatchingUrlsAsync<TType, TKey>(string url, TKey entityId = default)
            where TType : class, ISlug, IEntity<TKey>;
    }
}
