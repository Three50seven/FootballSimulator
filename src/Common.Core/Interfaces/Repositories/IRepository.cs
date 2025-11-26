using System;
using System.Threading.Tasks;

namespace Common.Core
{
    /// <summary>
    /// Generic repository for standard query and command operations.
    /// Pulls in commands from <see cref="ICommandRepository{TType, TKey}"/> and pulls in queries from <see cref="IQueryRepository{TType, TKey, TIncludes}"/>.
    /// Includes some "scoped" methods for Update and Delete commands using the Id key rather than the entity.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public interface IRepository<TType> : 
        ICommandRepository<TType>, 
        IQueryRepository<TType>,
        IRepository<TType, int>
        where TType : class
    {

    }

    /// <summary>
    /// Generic repository for standard query and command operations.
    /// Pulls in commands from <see cref="ICommandRepository{TType, TKey}"/> and pulls in queries from <see cref="IQueryRepository{TType, TKey, TIncludes}"/>.
    /// Includes some "scoped" methods for Update and Delete commands using the Id key rather than the entity.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TType, TKey> : 
        IRepository<TType, TKey, RepositoryIncludesDefaultOption>, 
        IQueryRepository<TType, TKey> 
        where TType : class
    {

    }

    /// <summary>
    /// Generic repository for standard query and command operations.
    /// Pulls in commands from <see cref="ICommandRepository{TType, TKey}"/> and pulls in queries from <see cref="IQueryRepository{TType, TKey, TIncludes}"/>.
    /// Includes some "scoped" methods for Update and Delete commands using the Id key rather than the entity.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TIncludes"></typeparam>
    public interface IRepository<TType, TKey, TIncludes> : 
        ICommandRepository<TType, TKey>, 
        IQueryRepository<TType, TKey, TIncludes>
        where TType : class
        where TIncludes : struct, Enum
    {
        TType Update(TKey id, Action<TType> callback, TIncludes includes = default, int userId = default, bool? recordChangeEvent = null);
        Task<TType> UpdateAsync(TKey id, Action<TType> callback, TIncludes includes = default, int userId = default, bool? recordChangeEvent = null);
        void Delete(TKey id, int userId = default, bool? recordChangeEvent = null);
        Task DeleteAsync(TKey id, int userId = default, bool? recordChangeEvent = null);
    }
}
