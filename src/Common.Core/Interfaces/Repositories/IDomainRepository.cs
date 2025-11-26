using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Core
{
    /// <summary>
    /// Generic repository for domain entities. 
    /// Includes all standard entity queries and commands as well Guid-based operations.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public interface IDomainRepository<TType> :
        IDomainRepository<TType, RepositoryIncludesDefaultOption>, 
        IRepository<TType>
        where TType : class
    {

    }

    /// <summary>
    /// Generic repository for domain entities. 
    /// Includes all standard entity queries and commands as well Guid-based operations.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TIncludes"></typeparam>
    public interface IDomainRepository<TType, TIncludes> : 
        IRepository<TType, int, TIncludes>,
        ICommandRepository<TType>
        where TType : class
        where TIncludes : struct, Enum
    {
        TType GetByGuid(Guid guid, TIncludes includes = default);
        Task<TType> GetByGuidAsync(Guid guid, TIncludes includes = default);
        int GetId(Guid guid);
        Task<int> GetIdAsync(Guid guid);
        Guid GetGuid(int id);
        Task<Guid> GetGuidAsync(int id);
        IEnumerable<TType> GetAll(IEnumerable<Guid> guids, TIncludes includes = default);
        Task<IEnumerable<TType>> GetAllAsync(IEnumerable<Guid> guids, TIncludes includes = default);
    }
}
