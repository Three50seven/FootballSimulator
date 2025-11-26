using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Core
{
    /// <summary>
    /// Generic repository for standard query actions.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public interface IQueryRepository<TType> : IQueryRepository<TType, int>
        where TType : class
    {

    }

    /// <summary>
    /// Generic repository for standard query actions.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IQueryRepository<TType, TKey> : IQueryRepository<TType, TKey, RepositoryIncludesDefaultOption>
        where TType : class
    {

    }

    /// <summary>
    /// Generic repository for standard query actions.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TIncludes"></typeparam>
    public interface IQueryRepository<TType, TKey, TIncludes> 
        where TType : class
        where TIncludes : struct, Enum
    {
        TType GetById(TKey id, TIncludes includes = default);
        Task<TType> GetByIdAsync(TKey id, TIncludes includes = default);
        IEnumerable<TType> GetAll(TIncludes includes = default);
        Task<IEnumerable<TType>> GetAllAsync(TIncludes includes = default);
        IEnumerable<TType> GetAll(IEnumerable<TKey> ids, TIncludes includes = default);
        Task<IEnumerable<TType>> GetAllAsync(IEnumerable<TKey> ids, TIncludes includes = default);
        int Count();
        Task<int> CountAsync();
    }
}
