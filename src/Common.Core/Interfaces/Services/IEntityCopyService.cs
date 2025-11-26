using Common.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IEntityCopyService<T> : IEntityCopyService<T, Guid, CommandResult>
         where T : class, IEntityGuid
    {

    }

    public interface IEntityCopyService<T, K, R>
        where T : class
        where R : CommandResult
    {
        R Copy(K id);
        Task<R> CopyAsync(K id);
    }
}
