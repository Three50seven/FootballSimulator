using Common.Core.Domain;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class LookupRepositoryExtensions
    {
        /// <summary>
        /// Lookup entity by Name and return its Id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lookupRepository"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async Task<int?> LookupIdByNameAsync<T>(this ILookupRepository<T> lookupRepository, string name)
            where T : class, IEntity<int>
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var lookupEntity = await lookupRepository.GetByNameAsync(name);
            return lookupEntity?.Id;
        }

        /// <summary>
        /// Lookup entity by Name and return its Id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lookupRepository"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int? LookupIdByName<T>(this ILookupRepository<T> lookupRepository, string name)
            where T : class, IEntity<int>
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var lookupEntity = lookupRepository.GetByName(name);
            return lookupEntity?.Id;
        }
    }
}
