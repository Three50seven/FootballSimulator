using System;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class QueryRepositoryExtensions
    {
        /// <summary>
        /// Lookup entity record by int value of supplied enum.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="repository"></param>
        /// <param name="enum"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static TEntity GetByEnum<TEntity, TEnum>(
            this IQueryRepository<TEntity> repository,
            TEnum @enum,
            RepositoryIncludesDefaultOption includes = default)
            where TEntity : class
            where TEnum : struct, Enum
        {
            return GetByEnum<TEntity, TEnum, RepositoryIncludesDefaultOption>(repository, @enum, includes);
        }

        /// <summary>
        /// Lookup entity record by int value of supplied enum.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="repository"></param>
        /// <param name="enum"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static Task<TEntity> GetByEnumAsync<TEntity, TEnum>(
            this IQueryRepository<TEntity> repository,
            TEnum @enum,
            RepositoryIncludesDefaultOption includes = default)
            where TEntity : class
            where TEnum : Enum
        {
            return GetByEnumAsync<TEntity, TEnum, RepositoryIncludesDefaultOption>(repository, @enum, includes);
        }

        /// <summary>
        /// Lookup entity record by int value of supplied enum.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TIncludes"></typeparam>
        /// <param name="repository"></param>
        /// <param name="enum"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static TEntity GetByEnum<TEntity, TEnum, TIncludes>(
            this IQueryRepository<TEntity, int, TIncludes> repository,
            TEnum @enum,
            TIncludes includes = default)
            where TEntity : class
            where TEnum : Enum
            where TIncludes : struct, Enum
        {
            return repository.GetById(@enum.ToInt(), includes);
        }

        /// <summary>
        /// Lookup entity record by int value of supplied enum.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TIncludes"></typeparam>
        /// <param name="repository"></param>
        /// <param name="enum"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static Task<TEntity> GetByEnumAsync<TEntity, TEnum, TIncludes>(
            this IQueryRepository<TEntity, int, TIncludes> repository,
            TEnum @enum,
            TIncludes includes = default)
            where TEntity : class
            where TEnum : Enum
            where TIncludes : struct, Enum
        {
            return repository.GetByIdAsync(@enum.ToInt(), includes);
        }


    }
}
