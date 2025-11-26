using Common.Core.Domain;
using Common.Core.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    public class SlugGenerator : ISlugGenerator
    {
        public SlugGenerator(IEntitySlugRepository entitySlugRepository)
        {
            EntitySlugRepository = entitySlugRepository;
        }

        protected IEntitySlugRepository EntitySlugRepository { get; }

        public virtual string Generate(string value)
        {
            return value.ToUrlFriendlyString();
        }

        public virtual Task<string> GenerateAsync(string value)
        {
            return Task.FromResult(Generate(value));
        }

        public virtual string GenerateUnique<TType, TKey>(string value, TKey entityId = default)
            where TType : class, ISlug, IEntity<TKey>
        {
            Guard.IsNotNull(value, nameof(value));

            value = Generate(value);

            var matchingValues = EntitySlugRepository.GetAnyMatchingUrls<TType, TKey>(value, entityId);

            return MakeUnique(value, matchingValues);
        }

        public virtual async Task<string> GenerateUniqueAsync<TType, TKey>(string value, TKey entityId = default)
            where TType : class, ISlug, IEntity<TKey>
        {
            Guard.IsNotNull(value, nameof(value));

            value = await GenerateAsync(value);

            var matchingValues = await EntitySlugRepository.GetAnyMatchingUrlsAsync<TType, TKey>(value, entityId);

            return MakeUnique(value, matchingValues);
        }

        protected virtual string MakeUnique(string value, IEnumerable<string> matchingValues)
        {
            if (matchingValues.Any())
            {
                string uniqueMaker = "";
                int seq = 0;
                while (matchingValues.Any(x => x == value + uniqueMaker))
                {
                    uniqueMaker = $"-{++seq}";
                }
                value += uniqueMaker;
            }

            return value;
        }
    }
}
