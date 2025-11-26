using Common.Core.Domain;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface ISlugGenerator
    {
        string Generate(string value);
        Task<string> GenerateAsync(string value);

        string GenerateUnique<TType, TKey>(string value, TKey entityId = default)
            where TType : class, ISlug, IEntity<TKey>;

        Task<string> GenerateUniqueAsync<TType, TKey>(string value, TKey entityId = default)
            where TType : class, ISlug, IEntity<TKey>;
    }
}
